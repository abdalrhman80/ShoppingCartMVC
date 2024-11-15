using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Specifications
{
	public class OrderHeaderSpecification : BaseSpecification<OrderHeader>
	{
		public OrderHeaderSpecification() : base()
		{
			AddIncludes(h => h.ApplicationUser);
		}

		public OrderHeaderSpecification(int id) : base(h => h.Id == id)
		{
			AddIncludes(h => h.ApplicationUser);
		}

        public OrderHeaderSpecification(string userId) : base(h => h.ApplicationUserId == userId)
        {
            //AddIncludes(h => h.ApplicationUser);
        }
    }
}
