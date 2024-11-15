using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCart.DAL.Models;

namespace ShoppingCart.PL.ViewModels
{
	public class ProductViewModel
	{
		public Product Product { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem> CategoryList { get; set; }
	}
}
