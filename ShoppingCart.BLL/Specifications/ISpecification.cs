using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Specifications
{
	public interface ISpecification<T> where T : class
	{
		// Signature For Prosperity For Where Condition
		public Expression<Func<T, bool>> Criteria { get; set; }

		// Signature For Prosperity For List Of Includes Expressions
		public List<Expression<Func<T, object>>> Includes { get; set; }
	}
}
