using ShoppingCart.BLL.Interfaces;
using ShoppingCart.DAL.Data;
using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Repositories
{
	public class ShoppingCartRepository : GenericRepository<UserShoppingCart>, IShoppingCartRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public ShoppingCartRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
		public int Increase(UserShoppingCart userShoppingCart, int count)
		{
			userShoppingCart.Count += count;
			return userShoppingCart.Count;
		}

		public int Decrease(UserShoppingCart userShoppingCart, int count)
		{
			userShoppingCart.Count -= count;
			return userShoppingCart.Count;
		}
	}
}
