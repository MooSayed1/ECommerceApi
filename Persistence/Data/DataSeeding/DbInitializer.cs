using System.Text.Json;
using Domain.Contracts;
using Domain.Entities.OrderEntities;
using Microsoft.AspNetCore.Identity;
using Persistance.Data.Contexts;

namespace Persistance.Data.DataSeeding;

public class DbInitializer(AppDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    : IDbInitializer
{
    public async Task InitializeDbAsync()
    {
        try
        {
            // if ((await _dbContext.Database.GetPendingMigrationsAsync()).Any()) // related to production
            // {
            await dbContext.Database.MigrateAsync();
            if (!dbContext.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync("../Persistence/Data/DataSeeding/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products != null)
                {
                    await dbContext.AddRangeAsync(products);
                }
            }

            if (!dbContext.ProductTypes.Any())
            {
                var productsTypesData = await File.ReadAllTextAsync("../Persistence/Data/DataSeeding/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(productsTypesData);
                if (types != null)
                {
                    await dbContext.AddRangeAsync(types);
                }
            }

            if (!dbContext.ProductBrands.Any())
            {
                string brandsPath = Path.Combine("../Persistence/Data/DataSeeding/brands.json");
                var productsBrandsData = await File.ReadAllTextAsync(brandsPath);
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(productsBrandsData);
                if (brands != null)
                {
                    await dbContext.AddRangeAsync(brands);
                }
            }

            if (!dbContext.DeliveryMethods.Any())
            {
                string brandsPath = Path.Combine("../Persistence/Data/DataSeeding/delivery.json");
                var methodsData = await File.ReadAllTextAsync(brandsPath);
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(methodsData);
                if (methods != null)
                {
                    await dbContext.AddRangeAsync(methods);
                }
            }

            await dbContext.SaveChangesAsync();
            //}
        }
        catch (Exception ex)
        {
            // Use a proper logger in a real application
            Console.WriteLine($"An error occurred during DB initialization: {ex.Message}");
        }
    }

    public async Task IdentitySeedAsync()
    {
        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        }


        if (!userManager.Users.Any())
        {
            var adminUser = new User
            {
                DisplayName = "Admin",
                UserName = "admin@example.com",
                Email = "admin@example.com",
                PhoneNumber = "0888888888"
            };
            await userManager.CreateAsync(adminUser, "Admin@123456");

            // Create super admin user
            var superAdminUser = new User
            {
                UserName = "superadmin@example.com",
                Email = "superadmin@example.com",
                EmailConfirmed = true,
                PhoneNumber = "0888888888"
            };
            await userManager.CreateAsync(superAdminUser, "SuperAdmin@123456");

            await userManager.AddToRoleAsync(adminUser, "Admin");
            await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
        }
    }
}