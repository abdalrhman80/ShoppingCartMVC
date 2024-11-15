using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Interfaces
{
	public interface IShoppingCartRepository : IGenericRepository<UserShoppingCart>
	{
		int Increase(UserShoppingCart userShoppingCart, int count);
		int Decrease(UserShoppingCart userShoppingCart, int count);
	}
}
