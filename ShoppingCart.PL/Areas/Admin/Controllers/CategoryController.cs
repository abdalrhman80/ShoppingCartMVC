using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.BLL.Interfaces;
using ShoppingCart.BLL.Repositories;
using ShoppingCart.BLL.Specifications;
using ShoppingCart.DAL.Data;
using ShoppingCart.DAL.Models;
using ShoppingCart.PL.Utilities;

namespace ShoppingCart.PL.Areas.Admin.Controllers
{
    [Authorize(Roles = UsersRoles.AdminRole)]
    [Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public CategoryController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		#region Index
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> GetCategoriesData()
		{
			var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

			return Json(new { data = categories });
		}
		#endregion

		#region Details
		public async Task<IActionResult> Details(int? id, string ViewName = "Details")
		{
			if (id is null) return BadRequest();

			var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id.Value);

			return category is null ? NotFound() : View(ViewName, category);
		}
		#endregion

		#region Create
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Category category)
		{
			if (ModelState.IsValid) // Server-Side Validation
			{
				await _unitOfWork.CategoryRepository.AddAsync(category);

				int result = await _unitOfWork.CompleteAsync();

				if (result > 0) TempData["Create"] = "Category Is Crated Successfully";

				return RedirectToAction(nameof(Index));
			}

			return View(category);
		}
		#endregion

		#region Edit
		[HttpGet]
		public async Task<IActionResult> Edit(int? id)
		{
			return await Details(id, "Edit");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Category category)
		{
			if (ModelState.IsValid) // Server-Side Validation
			{
				_unitOfWork.CategoryRepository.UpdateAsync(category);

				int result = await _unitOfWork.CompleteAsync();

				if (result > 0) TempData["Update"] = "Category Is Updated Successfully";

				return RedirectToAction(nameof(Index));
			}

			return View(category);
		}
		#endregion

		#region Delete
		[HttpDelete]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var categoryInDb = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

			if (categoryInDb is null)
				return Json(new { success = false, message = "Error While Deleting Category" });

			_unitOfWork.CategoryRepository.Remove(categoryInDb);

			await _unitOfWork.CompleteAsync();

			return Json(new { success = true, message = "Category Is Deleted Successfully" });
		}

		#endregion
	}
}
