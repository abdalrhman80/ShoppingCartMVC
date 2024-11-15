using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DAL.Data;
using ShoppingCart.PL.Utilities;
using System.Security.Claims;

namespace ShoppingCart.PL.Areas.Admin.Controllers
{
	[Authorize(Roles = UsersRoles.AdminRole)]
	[Area("Admin")]
	public class UsersController : Controller
	{
		private readonly ApplicationDbContext _dbContext;

		public UsersController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		#region Index
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult GetUsersData()
		{
			var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;

			var usersExceptCurrent = _dbContext.ApplicationUsers.Where(user => user.Id != userId).ToList();

			return Json(new { data = usersExceptCurrent });
		}
		#endregion

		#region Lockout
		public IActionResult LockUnlock(string? id)
		{
			var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == id);

			if (user is null) return NotFound();

			user.LockoutEnd = (user.LockoutEnd is null || user.LockoutEnd < DateTime.Now) ? DateTime.Now.AddMonths(1) : DateTime.Now;

			_dbContext.SaveChanges();

			return RedirectToAction("Index");
		}
		#endregion
	}
}
