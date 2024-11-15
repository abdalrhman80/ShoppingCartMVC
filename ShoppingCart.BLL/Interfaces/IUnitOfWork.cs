using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		public ICategoryRepository CategoryRepository { get; set; }
		public IProductRepository ProductRepository { get; set; }
		public IShoppingCartRepository ShoppingCartRepository { get; set; }
		public IOrderHeaderRepository OrderHeaderRepository { get; set; }
		public IOrderDetailsRepository OrderDetailsRepository { get; set; }
		public IApplicationUserRepository ApplicationUserRepository { get; set; }

		Task<int> CompleteAsync();
	}
}
