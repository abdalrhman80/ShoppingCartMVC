using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShoppingCart.BLL.Interfaces;
using ShoppingCart.BLL.Specifications;
using ShoppingCart.DAL.Models;
using ShoppingCart.PL.Utilities;
using ShoppingCart.PL.ViewModels;
using Stripe.Checkout;
using Stripe.Climate;
using System.Security.Claims;

namespace ShoppingCart.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private const string SessionId = "SessionItems";

        public CartController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;

            var specification = new ShoppingCartSpecification(userId);

            var shoppingCartVM = new ShoppingCartViewModel
            {
                CartItems = await _unitOfWork.ShoppingCartRepository.GetAllWithSpecificationAsync(specification),
                OrderHeader = new()
            };

            foreach (var cartItem in shoppingCartVM.CartItems)
            {
                shoppingCartVM.TotalPrice += (cartItem.Count * cartItem.Product.Price);
            }

            return View(shoppingCartVM);
        }
        #endregion

        #region Plus
        public async Task<IActionResult> Plus(int itemId)
        {
            var cartItem = await _unitOfWork.ShoppingCartRepository.GetByIdAsync(itemId);

            if (cartItem is null) return NotFound();

            _unitOfWork.ShoppingCartRepository.Increase(cartItem, 1);

            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Minus
        public async Task<IActionResult> Minus(int itemId)
        {
            var cartItem = await _unitOfWork.ShoppingCartRepository.GetByIdAsync(itemId);

            if (cartItem is null) return NotFound();

            var userCartSpecification = new ShoppingCartSpecification(cartItem.ApplicationUserId);

            var cart = await _unitOfWork.ShoppingCartRepository.GetAllWithSpecificationAsync(userCartSpecification);

            if (cartItem.Count <= 1)
            {
                _unitOfWork.ShoppingCartRepository.Remove(cartItem);

                HttpContext.Session.SetInt32(SessionId, cart.Count() - 1);
            }
            else
            {
                _unitOfWork.ShoppingCartRepository.Decrease(cartItem, 1);
            }

            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int itemId)
        {
            var cartItem = await _unitOfWork.ShoppingCartRepository.GetByIdAsync(itemId);

            if (cartItem is null) return NotFound();

            _unitOfWork.ShoppingCartRepository.Remove(cartItem);

            await _unitOfWork.CompleteAsync();

            var userCartSpecification = new ShoppingCartSpecification(cartItem.ApplicationUserId);

            var cart = await _unitOfWork.ShoppingCartRepository.GetAllWithSpecificationAsync(userCartSpecification);

            HttpContext.Session.SetInt32(SessionId, cart.Count());

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Summary
        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartSpecification = new ShoppingCartSpecification(userId);

            var shoppingCartVM = new ShoppingCartViewModel
            {
                CartItems = await _unitOfWork.ShoppingCartRepository.GetAllWithSpecificationAsync(cartSpecification),
                OrderHeader = new()
            };

            var userSpecification = new ApplicationUserSpecification(userId);

            shoppingCartVM.OrderHeader.ApplicationUser = await _unitOfWork.ApplicationUserRepository.GetEntityWithSpecificationAsync(userSpecification);

            shoppingCartVM.OrderHeader.UserName = shoppingCartVM.OrderHeader.ApplicationUser.FullName;
            shoppingCartVM.OrderHeader.Address = shoppingCartVM.OrderHeader.ApplicationUser.Address;
            shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City;
            shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;

            foreach (var cartItem in shoppingCartVM.CartItems)
            {
                shoppingCartVM.OrderHeader.TotalPrice += (cartItem.Count * cartItem.Product.Price);
            }

            return View(shoppingCartVM);
        }
        #endregion

        #region Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(ShoppingCartViewModel shoppingCartViewModel)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartSpecification = new ShoppingCartSpecification(userId);

            var shoppingCartVM = new ShoppingCartViewModel
            {
                CartItems = await _unitOfWork.ShoppingCartRepository.GetAllWithSpecificationAsync(cartSpecification),
                OrderHeader = shoppingCartViewModel.OrderHeader,
            };

            shoppingCartVM.OrderHeader.OrderStatus = Status.Pending;
            shoppingCartVM.OrderHeader.PaymentStatus = Status.Pending;
            shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            shoppingCartVM.OrderHeader.ApplicationUserId = userId;

            foreach (var cartItem in shoppingCartVM.CartItems)
            {
                shoppingCartVM.OrderHeader.TotalPrice += (cartItem.Count * cartItem.Product.Price);
            }

            await _unitOfWork.OrderHeaderRepository.AddAsync(shoppingCartVM.OrderHeader);

            await _unitOfWork.CompleteAsync();

            foreach (var item in shoppingCartVM.CartItems)
            {
                var orderDetails = new OrderDetails
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = shoppingCartVM.OrderHeader.Id,
                    Price = item.Product.Price,
                    Quantity = item.Count
                };

                await _unitOfWork.OrderDetailsRepository.AddAsync(orderDetails);
                await _unitOfWork.CompleteAsync();
            }

            var baseUrl = _configuration["BaseUrl"];

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{baseUrl}customer/cart/orderconfirmation?id={shoppingCartVM.OrderHeader.Id}",
                CancelUrl = $"{baseUrl}customer/cart/index",
            };

            foreach (var item in shoppingCartVM.CartItems)
            {
                var sessionLineItemOptions = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count,
                };

                options.LineItems.Add(sessionLineItemOptions);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            shoppingCartVM.OrderHeader.SessionId = session.Id;
            shoppingCartVM.OrderHeader.PaymentIntentId = session.PaymentIntentId;

            await _unitOfWork.CompleteAsync();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        #endregion

        #region OrderConfirmation
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var orderHeader = await _unitOfWork.OrderHeaderRepository.GetByIdAsync(id);

            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeaderRepository.UpdateOrderStatus(id, Status.Approve);
                _unitOfWork.OrderHeaderRepository.UpdatePaymentStatus(id, Status.Approve);
                orderHeader.PaymentIntentId = session.PaymentIntentId;
                await _unitOfWork.CompleteAsync();
            }

            var cartSpecification = new ShoppingCartSpecification(orderHeader.ApplicationUserId);

            var shoppingCarts = await _unitOfWork.ShoppingCartRepository.GetAllWithSpecificationAsync(cartSpecification);

            _unitOfWork.ShoppingCartRepository.RemoveRange(shoppingCarts);

            await _unitOfWork.CompleteAsync();

            HttpContext.Session.Clear();

            //return View(id);
            return RedirectToAction("Details", "CustomerOrder", new { orderId = id });
        }

        #endregion
    }
}
