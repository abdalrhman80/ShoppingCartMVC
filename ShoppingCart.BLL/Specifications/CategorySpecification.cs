using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BLL.Specifications
{
	public class CategorySpecification : BaseSpecification<Category>
	{
		// GetAll
		public CategorySpecification() : base()
		{
		}

		// GetById
		public CategorySpecification(int id) : base(c => c.Id == id)
		{
		}
	}
}
