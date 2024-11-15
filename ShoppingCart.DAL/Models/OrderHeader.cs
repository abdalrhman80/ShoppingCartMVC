using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DAL.Models
{
	public class OrderHeader
	{
		public int Id { get; set; }

		public string ApplicationUserId { get; set; }

		[ValidateNever]
		[ForeignKey("ApplicationUserId")]
		public ApplicationUser ApplicationUser { get; set; }

		#region UserData
		public string UserName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string PhoneNumber { get; set; }
		#endregion

		public DateTime OrderDate { get; set; }

		public DateTime ShippingDate { get; set; }

		public string? OrderStatus { get; set; }

		public string? PaymentStatus { get; set; }

		public decimal TotalPrice { get; set; }

		public string? TrackingNumber { get; set; }

		public string? Carrier { get; set; }

		public DateTime PaymentDate { get; set; }

		#region Stripe Props
		public string? SessionId { get; set; }
		public string? PaymentIntentId { get; set; }
		#endregion
	}
}
