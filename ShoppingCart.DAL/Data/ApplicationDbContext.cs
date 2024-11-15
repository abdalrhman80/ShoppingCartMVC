using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DAL.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public DbSet<UserShoppingCart> ShoppingCarts { get; set; }
		public DbSet<OrderHeader> OrderHeaders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<ApplicationUser>().ToTable("Users");

			builder.Entity<IdentityRole>().ToTable("Roles");

			builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles").HasKey(r => new { r.UserId, r.RoleId });

			builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");

			builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins").HasKey(l => new { l.LoginProvider, l.ProviderKey });

			builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

			builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens").HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
		}
	}
}
