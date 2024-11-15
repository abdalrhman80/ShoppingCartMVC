using Microsoft.EntityFrameworkCore;
using ShoppingCart.BLL.Interfaces;
using ShoppingCart.DAL.Data;
using ShoppingCart.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Repositories
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private readonly ApplicationDbContext _dbContext;

		public ICategoryRepository CategoryRepository { get; set; }
		public IProductRepository ProductRepository { get; set; }
		public IShoppingCartRepository ShoppingCartRepository { get; set; }
		public IOrderHeaderRepository OrderHeaderRepository { get; set; }
		public IOrderDetailsRepository OrderDetailsRepository { get; set; }
		public IApplicationUserRepository ApplicationUserRepository { get; set; }

		public UnitOfWork(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
			CategoryRepository = new CategoryRepository(dbContext);
			ProductRepository = new ProductRepository(dbContext);
			ShoppingCartRepository = new ShoppingCartRepository(dbContext);
			OrderDetailsRepository = new OrderDetailsRepository(dbContext);
			OrderHeaderRepository = new OrderHeaderRepository(dbContext);
			ApplicationUserRepository = new ApplicationUserRepository(dbContext);
		}

		public async Task<int> CompleteAsync() => await _dbContext.SaveChangesAsync();

		public void Dispose() => _dbContext.Dispose();
	}
}
