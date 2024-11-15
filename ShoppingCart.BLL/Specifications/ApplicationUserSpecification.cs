using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Specifications
{
	public class ApplicationUserSpecification : BaseSpecification<ApplicationUser>
	{
		public ApplicationUserSpecification() : base() { }

		public ApplicationUserSpecification(string id) : base(u => u.Id == id) { }
	}
}
