using ShoppingCart.DAL.Models;

namespace ShoppingCart.PL.ViewModels
{
	public class ShoppingCartViewModel
	{
		public IEnumerable<UserShoppingCart> CartItems { get; set; }
		public decimal TotalPrice { get; set; }
		public OrderHeader OrderHeader { get; set; }
	}
}
