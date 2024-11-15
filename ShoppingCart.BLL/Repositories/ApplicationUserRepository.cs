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
	public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
	}
}
