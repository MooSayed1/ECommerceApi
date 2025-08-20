using System.Text.Json;
using Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using Persistance.Data.Contexts;

namespace Persistance.Data.DataSeeding;

public class DbInitializer : IDbInitializer
{
    private readonly AppDbContext _dbcontext;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public DbInitializer(AppDbContext dbcontext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _dbcontext = dbcontext;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task InitializeDbAsync()
    {
        try
        {
            if ((await _dbcontext.Database.GetPendingMigrationsAsync()).Any())
            {
                await _dbcontext.Database.MigrateAsync();
                if (!_dbcontext.Products.Any())
                {
                    var productsData = await File.ReadAllTextAsync("../Persistence/Data/DataSeeding/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    if (products != null)
                    {
                        await _dbcontext.AddRangeAsync(products);
                    }
                }

                if (!_dbcontext.ProductTypes.Any())
                {
                    var productsTypesData = await File.ReadAllTextAsync("../Persistence/Data/DataSeeding/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(productsTypesData);
                    if (types != null)
                    {
                        await _dbcontext.AddRangeAsync(types);
                    }
                }

                if (!_dbcontext.ProductBrands.Any())
                {
                    string brandsPath = Path.Combine("../Persistence/Data/DataSeeding/brands.json");
                    var productsBrandsData = await File.ReadAllTextAsync(brandsPath);
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(productsBrandsData);
                    if (brands != null)
                    {
                        await _dbcontext.AddRangeAsync(brands);
                    }
                }

                await _dbcontext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            // Use a proper logger in a real application
            Console.WriteLine($"An error occurred during DB initialization: {ex.Message}");
        }
    }

    public async Task IdentitySeedAsync()
    {
        if (!_roleManager.Roles.Any())
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        }


        if (!_userManager.Users.Any())
        {
            var adminUser = new User
            {
                DisplayName = "Admin",
                UserName = "admin@example.com",
                Email = "admin@example.com",
                PhoneNumber = "0888888888"
            };
            await _userManager.CreateAsync(adminUser, "Admin@123456");

            // Create super admin user
            var superAdminUser = new User
            {
                UserName = "superadmin@example.com",
                Email = "superadmin@example.com",
                EmailConfirmed = true,
                PhoneNumber = "0888888888"
            };
            await _userManager.CreateAsync(superAdminUser, "SuperAdmin@123456");

            await _userManager.AddToRoleAsync(adminUser, "Admin");
            await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
            
        }
    }
}