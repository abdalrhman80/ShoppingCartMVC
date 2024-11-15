using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DAL.Models
{
	public class OrderDetails
	{
		public int Id { get; set; }

		public int OrderHeaderId { get; set; }

		[ValidateNever]
		[ForeignKey("OrderHeaderId")]
		public OrderHeader OrderHeader { get; set; }

		public int ProductId { get; set; }

		[ValidateNever]
		[ForeignKey("ProductId")]
		public Product Product { get; set; }

		public int Quantity { get; set; }

		public decimal Price { get; set; }
	}
}
