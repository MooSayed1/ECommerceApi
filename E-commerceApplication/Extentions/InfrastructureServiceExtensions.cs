using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistance.Data.Contexts;
using Persistance.Data.DataSeeding;
using Persistance.Repositories;
using StackExchange.Redis;

namespace E_commerceApplication.Extentions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IDbInitializer, DbInitializer>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IConnectionMultiplexer>(_ => 
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!)
            );
        
        services.AddScoped<IBasketRepository, BasketRepository>();
        return services;
    }
}