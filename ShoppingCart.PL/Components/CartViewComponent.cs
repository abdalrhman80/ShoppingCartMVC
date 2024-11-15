using Microsoft.AspNetCore.Mvc;
using ShoppingCart.BLL.Interfaces;
using ShoppingCart.BLL.Specifications;
using System.Security.Claims;

namespace ShoppingCart.PL.Components
{
	public class CartViewComponent : ViewComponent
	{
		private readonly IUnitOfWork _unitOfWork;
		private const string SessionId = "SessionItems";

		public CartViewComponent(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);

			if (userId != null)
			{
				if (HttpContext.Session.GetInt32(SessionId) != null)
				{
					return View(HttpContext.Session.GetInt32(SessionId));
				}
				else
				{
					var userCartSpecification = new ShoppingCartSpecification(userId.Value);

					var cart = await _unitOfWork.ShoppingCartRepository.GetAllWithSpecificationAsync(userCartSpecification);

					HttpContext.Session.SetInt32(SessionId, cart.Count());

					return View(HttpContext.Session.GetInt32(SessionId));
				}
			}
			else
			{
				HttpContext.Session.Clear();
				return View(0);
			}
		}
	}
}
