using ShoppingCart.BLL.Specifications;
using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
		#region Without Specification
		Task<IEnumerable<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);
		Task AddAsync(T item);
		void Remove(T item);
		void RemoveRange(IEnumerable<T> items);
		#endregion

		#region With Specification
		Task<IEnumerable<T>> GetAllWithSpecificationAsync(ISpecification<T> specification);
		Task<T> GetEntityWithSpecificationAsync(ISpecification<T> specification);
		#endregion
	}
}
