using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using ShoppingCart.BLL.Interfaces;
using ShoppingCart.BLL.Specifications;
using ShoppingCart.DAL.Models;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace ShoppingCart.PL.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private const string SessionId = "SessionItems";

		public HomeController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		#region Index
		public async Task<IActionResult> Index(int? page)
		{
			var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)

			var specification = new ProductSpecification();

			var products = await _unitOfWork.ProductRepository.GetAllWithSpecificationAsync(specification);

			var onePageOfProducts = products.ToPagedList(pageNumber, 10); // will only contain 10 products max because of the pageSize

			return View(onePageOfProducts);
		}
		#endregion

		#region Details
		[HttpGet]
		public async Task<IActionResult> Details([FromRoute(Name = "id")] int? productId)
		{
			if (productId is null) return BadRequest();

			var specification = new ProductSpecification(productId.Value);

			var product = await _unitOfWork.ProductRepository.GetEntityWithSpecificationAsync(specification);

			if (product is null) return NotFound();

			var userCart = new UserShoppingCart()
			{
				ProductId = productId.Value,
				Product = product,
				Count = 1
			};

			return View(userCart);
		}
		#endregion

		#region AddToCart
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> AddToCart([FromForm] UserShoppingCart shoppingCart)
		{
			if (shoppingCart == null) return BadRequest();

			var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;

			shoppingCart.ApplicationUserId = userId;

			var cartItemSpecification = new ShoppingCartSpecification(userId, shoppingCart.ProductId);

			var userCartItem = await _unitOfWork.ShoppingCartRepository.GetEntityWithSpecificationAsync(cartItemSpecification);


			if (userCartItem is null)
			{
				await _unitOfWork.ShoppingCartRepository.AddAsync(shoppingCart);

				await _unitOfWork.CompleteAsync();

				var userCartSpecification = new ShoppingCartSpecification(userId);

				var cart = await _unitOfWork.ShoppingCartRepository.GetAllWithSpecificationAsync(userCartSpecification);

				HttpContext.Session.SetInt32(SessionId, cart.Count());
			}
			else
			{
				_unitOfWork.ShoppingCartRepository.Increase(userCartItem, shoppingCart.Count);
				//HttpContext.Session.SetInt32(SessionId, cart.Count() + shoppingCart.Count);
				await _unitOfWork.CompleteAsync();
			}

			//await _unitOfWork.CompleteAsync();

			return RedirectToAction(nameof(Index));
		}
		#endregion
	}
}
