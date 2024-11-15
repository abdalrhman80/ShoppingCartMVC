using Microsoft.EntityFrameworkCore;
using ShoppingCart.BLL.Interfaces;
using ShoppingCart.BLL.Specifications;
using ShoppingCart.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly DbSet<T> _dbSet;

		public GenericRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<T>();
		}

		#region Without Specification
		public async Task<IEnumerable<T>> GetAllAsync()
			=> await _dbSet.ToListAsync();

		public async Task<T> GetByIdAsync(int id)
			=> await _dbSet.FindAsync(id);

		public async Task AddAsync(T item)
			=> await _dbSet.AddAsync(item);

		public void Remove(T item)
			=> _dbSet.Remove(item);		
		
		public void RemoveRange(IEnumerable<T> items)
			=> _dbSet.RemoveRange(items);
		#endregion

		#region With Specification
		public async Task<IEnumerable<T>> GetAllWithSpecificationAsync(ISpecification<T> specification)
			=> await ApplySpecification(specification).ToListAsync();

		public async Task<T> GetEntityWithSpecificationAsync(ISpecification<T> specification)
			=> await ApplySpecification(specification).FirstOrDefaultAsync();

		private IQueryable<T> ApplySpecification(ISpecification<T> specification)
			=> SpecificationEvaluator<T>.GetQuery(_dbSet, specification);
		#endregion
	}
}
