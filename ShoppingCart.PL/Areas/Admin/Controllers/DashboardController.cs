using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.BLL.Interfaces;
using ShoppingCart.BLL.Specifications;
using ShoppingCart.PL.Utilities;
using System.Security.Claims;

namespace ShoppingCart.PL.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = UsersRoles.AdminRole)]
	public class DashboardController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public DashboardController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IActionResult> Index()
		{
			var orders = await _unitOfWork.OrderHeaderRepository.GetAllAsync();

			ViewBag.Orders = orders.Count();

			ViewBag.ApprovedOrders = orders.Where(o => o.OrderStatus == Status.Approve).Count();

			var currentUserId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;

			ViewBag.Users = (await _unitOfWork.ApplicationUserRepository.GetAllAsync()).Where(x => x.Id != currentUserId).Count();

			ViewBag.Products = (await _unitOfWork.ProductRepository.GetAllAsync()).Count();

			return View();
		}
	}
}
