using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Specifications
{
	public class BaseSpecification<T> : ISpecification<T> where T : class
	{
		public Expression<Func<T, bool>> Criteria { get; set; }
		public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();

		public BaseSpecification()
		{
		}

		public BaseSpecification(Expression<Func<T, bool>> criteria)
		{
			Criteria = criteria;
		}

		protected void AddIncludes(Expression<Func<T, object>> includeExpression)
		{
			Includes.Add(includeExpression);
		}
	}
}
