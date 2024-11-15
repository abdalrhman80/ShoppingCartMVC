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
	public class OrderDetailsRepository : GenericRepository<OrderDetails>, IOrderDetailsRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public OrderDetailsRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public void Update(OrderDetails orderDetails)
		{
			_dbContext.OrderDetails.Update(orderDetails);
		}
	}
}
