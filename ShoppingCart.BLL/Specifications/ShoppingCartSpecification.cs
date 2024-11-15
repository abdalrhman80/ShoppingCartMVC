using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Specifications
{
	public class ShoppingCartSpecification : BaseSpecification<UserShoppingCart>
	{
		public ShoppingCartSpecification() : base()
		{
		}

		public ShoppingCartSpecification(string userId) : base(c => c.ApplicationUserId == userId)
		{
			AddIncludes(c => c.Product);
			AddIncludes(c => c.Product.Category);
		}

		public ShoppingCartSpecification(string userId, int productId) : base(c => c.ApplicationUserId == userId && c.ProductId == productId)
		{
		}
	}
}
