using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCart.BLL.Interfaces;
using ShoppingCart.BLL.Services.Interfaces;
using ShoppingCart.BLL.Specifications;
using ShoppingCart.DAL.Models;
using ShoppingCart.PL.Utilities;
using ShoppingCart.PL.ViewModels;

namespace ShoppingCart.PL.Areas.Admin.Controllers
{
    [Authorize(Roles = UsersRoles.AdminRole)]
    [Area("Admin")]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private const string FolderPath = @"WebImages\Products";
		private readonly IFileService _fileService;

		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IFileService fileService)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
			_fileService = fileService;
		}

		#region Index
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> GetProductsData()
		{
			var specification = new ProductSpecification();

			var products = await _unitOfWork.ProductRepository.GetAllWithSpecificationAsync(specification);

			return Json(new { data = products });
		}
		#endregion

		#region Details
		public async Task<IActionResult> Details(int? id, string ViewName = "Details")
		{
			if (id is null) return BadRequest();

			var specification = new ProductSpecification(id.Value);

			var product = await _unitOfWork.ProductRepository.GetEntityWithSpecificationAsync(specification);

			if (product is null) return NotFound();

			var productViewModel = new ProductViewModel()
			{
				Product = product,
				CategoryList = _unitOfWork.CategoryRepository.GetAllAsync().Result.
				Select(categoryItem => new SelectListItem
				{
					Text = categoryItem.Name,
					Value = categoryItem.Id.ToString()
				})
			};

			//TempData["Image"] = productViewModel.Product.Image.Split("Products\\")[1];

			return View(ViewName, productViewModel);
		}
		#endregion

		#region Create
		[HttpGet]
		public IActionResult Create()
		{
			var productViewModel = new ProductViewModel()
			{
				Product = new Product(),
				CategoryList = _unitOfWork.CategoryRepository.GetAllAsync().Result.
				Select(categoryItem => new SelectListItem
				{
					Text = categoryItem.Name,
					Value = categoryItem.Id.ToString()
				})
			};

			return View(productViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductViewModel productViewModel, IFormFile file)
		{
			if (ModelState.IsValid) // Server-Side Validation
			{
				if (file is not null)
				{
					var imagePath = await _fileService.SaveFileAsync(file, _webHostEnvironment.WebRootPath, FolderPath);

					productViewModel.Product.Image = imagePath;
				}

				await _unitOfWork.ProductRepository.AddAsync(productViewModel.Product);

				int result = await _unitOfWork.CompleteAsync();

				if (result > 0) TempData["Create"] = "Product Is Crated Successfully";

				return RedirectToAction(nameof(Index));
			}

			return View(productViewModel.Product);
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
		public async Task<IActionResult> Edit(ProductViewModel productViewModel, IFormFile? file)
		{
			if (ModelState.IsValid) // Server-Side Validation
			{
				if (file is not null)
				{
					if (!string.IsNullOrEmpty(productViewModel.Product.Image))
						_fileService.DeleteFile(_webHostEnvironment.WebRootPath, productViewModel.Product.Image);

					var filePath = await _fileService.SaveFileAsync(file, _webHostEnvironment.WebRootPath, FolderPath);

					productViewModel.Product.Image = filePath;
				}

				_unitOfWork.ProductRepository.UpdateAsync(productViewModel.Product);

				int result = await _unitOfWork.CompleteAsync();

				if (result > 0) TempData["Update"] = "Product Is Updated Successfully";

				return RedirectToAction(nameof(Index));
			}

			return View(productViewModel.Product);
		}
		#endregion

		#region Delete
		[HttpDelete]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var productInDb = await _unitOfWork.ProductRepository.GetByIdAsync(id);

			if (productInDb is null)
				return Json(new { success = false, message = "Error While Deleting Product" });

			_unitOfWork.ProductRepository.Remove(productInDb);

			if (!string.IsNullOrEmpty(productInDb.Image))
				_fileService.DeleteFile(_webHostEnvironment.WebRootPath, productInDb.Image);

			await _unitOfWork.CompleteAsync();

			return Json(new { success = true, message = "Product Is Deleted Successfully" });
		}
		#endregion
	}
}
