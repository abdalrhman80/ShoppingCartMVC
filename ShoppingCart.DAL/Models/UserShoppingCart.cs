using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.DAL.Models
{
	public class UserShoppingCart
	{
		public int Id { get; set; } // CartItemId

		public int ProductId { get; set; }

		[ForeignKey("ProductId")]
		[ValidateNever]
		public Product Product { get; set; }

		[Range(1, 100, ErrorMessage = "You Must Enter Value Between 1 To 100")]
		public int Count { get; set; }

		[Column("UserId")]
		public string ApplicationUserId { get; set; }

		[ForeignKey("ApplicationUserId")]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }
	}
}
