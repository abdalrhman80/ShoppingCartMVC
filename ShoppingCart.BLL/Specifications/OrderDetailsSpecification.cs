using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Specifications
{
	public class OrderDetailsSpecification : BaseSpecification<OrderDetails>
	{
        public OrderDetailsSpecification(int orderHeaderId) : base(d => d.OrderHeaderId == orderHeaderId)
        {
            AddIncludes(d => d.Product);
        }
    }
}
