using ShoppingCart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingCart.DAL.Data
{
    public class DataSeedInitializer
    {
        public static async Task SeedDataAsync(ApplicationDbContext dbContext)
        {
            #region Categories
            if (!dbContext.Categories.Any()) // Check If No Data in Database
            {
                // 1-Serialize Data (Read and Covert The Data in File To a String)
                var categoriesData = File.ReadAllText("../ShoppingCart.DAL/Data/DataSeed/Categories.json");

                // 2-Deserialize Data (Convert The String Data To List Of Specific Object)
                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

                if (categories?.Count > 0) // Check categories is not null & more than 0
                {
                    // 3-Add Data In Database
                    foreach (var category in categories)
                        await dbContext.Set<Category>().AddAsync(category);

                    // 4- SaveChanges
                    await dbContext.SaveChangesAsync();
                }
            }
            #endregion
        }
    }
}
