using System.Text.Json;
using Domain.Contracts;
using Persistance.Data.Contexts;

namespace Persistance.Data.DataSeeding;

public class DbInitializer : IDbInitializer
{
    private readonly AppDbContext _dbcontext;

    public DbInitializer(AppDbContext dbcontext)
    {
        _dbcontext = dbcontext;
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
}