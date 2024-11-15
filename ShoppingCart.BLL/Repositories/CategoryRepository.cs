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
	public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public void UpdateAsync(Category updatedCategory)
		{
			var category = _dbContext.Categories.FirstOrDefault(c => c.Id == updatedCategory.Id);

			if (category != null)
			{
				category.Name = updatedCategory.Name;
				category.Description = updatedCategory.Description;
				category.CreatedTime = updatedCategory.CreatedTime;
			}
		}
	}
}
