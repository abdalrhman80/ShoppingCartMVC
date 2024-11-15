using Microsoft.EntityFrameworkCore;
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
	public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public void UpdateAsync(Product updatedProduct)
		{
			var product = _dbContext.Products.FirstOrDefault(c => c.Id == updatedProduct.Id);

			if (product != null)
			{
				product.Name = updatedProduct.Name;
				product.Description = updatedProduct.Description;
				product.Price = updatedProduct.Price;
				product.Image = updatedProduct.Image;
				product.CategoryId = updatedProduct.CategoryId;
			}
		}
	}
}
