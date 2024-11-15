using ShoppingCart.DAL.Models;

namespace ShoppingCart.PL.ViewModels
{
	public class OrderViewModel
	{
		public OrderHeader OrderHeader { get; set; }
		public IEnumerable<OrderDetails> OrderDetails { get; set; }
	}
}
