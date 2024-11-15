using Microsoft.AspNetCore.Mvc;
using ShoppingCart.BLL.Interfaces;
using ShoppingCart.BLL.Specifications;
using ShoppingCart.PL.ViewModels;
using System.Security.Claims;

namespace ShoppingCart.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CustomerOrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerOrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;

            var orderHeaderSpecification = new OrderHeaderSpecification(userId);

            var orders = await _unitOfWork.OrderHeaderRepository.GetAllWithSpecificationAsync(orderHeaderSpecification);

            return View(orders);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int orderId)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;

            var orderHeaderSpecification = new OrderHeaderSpecification(orderId);
            var orderDetailsSpecification = new OrderDetailsSpecification(orderId);

            var orderHeader = await _unitOfWork.OrderHeaderRepository.GetEntityWithSpecificationAsync(orderHeaderSpecification);
            var orderDetails = await _unitOfWork.OrderDetailsRepository.GetAllWithSpecificationAsync(orderDetailsSpecification);

            if (orderHeader == null) return NotFound();

            if (orderHeader.ApplicationUserId != userId) return NotFound();

            var orderViewModel = new OrderViewModel
            {
                OrderHeader = orderHeader,
                OrderDetails = orderDetails,
            };

            return View(orderViewModel);
        }
        #endregion
    }
}
