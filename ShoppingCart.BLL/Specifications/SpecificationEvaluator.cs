using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Specifications
{
	public static class SpecificationEvaluator<T> where T : class
	{
		public static IQueryable<T> GetQuery(IQueryable<T> dbSet, ISpecification<T> specification)
		{
			var query = dbSet;

			query = (specification.Criteria is not null) ? query.Where(specification.Criteria) : query;

			query = specification.Includes.Aggregate(query, (currentQuery, includeExpression)
				=> currentQuery.Include(includeExpression));

			return query;
		}
	}
}
