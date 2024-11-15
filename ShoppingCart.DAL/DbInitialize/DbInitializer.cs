using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCart.DAL.Data;
using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DAL.DbInitialize
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ILogger<DbInitializer> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private const string AdminRole = "Admin";
        private const string EditorRole = "Editor";
        private const string CustomerRole = "Customer";

        public DbInitializer(
            ApplicationDbContext dbContext,
            ILogger<DbInitializer> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            #region Update Database (Migrations)
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Count() > 0)
                    await _dbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error Occurred During Appling The Migration");
            }
            #endregion

            #region Create Admin User
            if (!await _roleManager.RoleExistsAsync(AdminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(AdminRole));
                await _roleManager.CreateAsync(new IdentityRole(EditorRole));
                await _roleManager.CreateAsync(new IdentityRole(CustomerRole));

                var newAdmin = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "Admin@gmail.com",
                    FullName = "Administrator",
                    PhoneNumber = "01067377533",
                    Address = @"Salah Salem, Beni-Suif, Beni-Suif",
                    City = "Beni-Suif",
                };

                await _userManager.CreateAsync(newAdmin, "P@$$w0rd");

                var administrator = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.FullName == newAdmin.FullName);

                await _userManager.AddToRoleAsync(administrator, AdminRole);
            }
            #endregion
        }
    }
}
