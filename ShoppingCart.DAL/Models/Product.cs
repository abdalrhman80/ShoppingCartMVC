using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShoppingCart.DAL.Models
{
	public class Product
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string Description { get; set; }
		[ValidateNever]
		public string Image { get; set; }
		[Required]
		public decimal Price { get; set; }

		#region Relation
		[Required]
		[ForeignKey("Category")]
		[DisplayName("Category")]
		public int CategoryId { get; set; }
		[ValidateNever]
		public Category Category { get; set; }
		#endregion
	}
}
