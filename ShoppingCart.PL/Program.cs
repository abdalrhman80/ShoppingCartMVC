using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShoppingCart.BLL.Interfaces;
using ShoppingCart.BLL.Repositories;
using ShoppingCart.BLL.Services.Interfaces;
using FileService = ShoppingCart.BLL.Services.Implementation.FileService;
using ShoppingCart.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ShoppingCart.PL.Utilities;
using ShoppingCart.DAL.Models;
using ShoppingCart.PL.Options;
using Stripe;
using ShoppingCart.DAL.DbInitialize;

namespace ShoppingCart.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Create Host
            var builder = WebApplication.CreateBuilder(args);
            #endregion

            #region DI Container
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services
             .AddIdentity<ApplicationUser, IdentityRole>(options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5))
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultUI()
             .AddDefaultTokenProviders();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            // Database Initializer
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            //Configuring Session Services in ASP.NET Core
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            builder.Services.Configure<StripeOptions>(builder.Configuration.GetSection("Stripe"));
            #endregion

            #region Build Project
            var app = builder.Build();
            #endregion

            #region Database Initializer
            using var scope = app.Services.CreateScope();

            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            
            await dbInitializer.InitializeAsync();
            #endregion

            #region Middlewares
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            //Configuring Session Middleware in ASP.NET Core
            //It should and must be configured after UseRouting and before MapControllerRoute
            app.UseSession();

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();
            #endregion

            #region Routeing
            app.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                    name: "Admin",
                    pattern: "{area=Admin}/{controller=Dashboard}/{action=Index}/{id?}");
            #endregion

            app.Run();
        }
    }
}