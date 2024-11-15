using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Interfaces
{
	public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
	{
		public void Update(OrderHeader orderHeader);
		public void UpdateOrderStatus(int id, string orderStatus);
		public void UpdatePaymentStatus(int id, string paymentStatus);
	}
}
