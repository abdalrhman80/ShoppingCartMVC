using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Interfaces
{
	public interface IProductRepository : IGenericRepository<Product>
	{
		void UpdateAsync(Product updatedProduct);
	}
}
