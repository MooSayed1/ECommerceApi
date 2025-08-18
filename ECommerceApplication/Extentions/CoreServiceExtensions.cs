using Services;
using Services.Abstraction.Interfaces;
using Services.MappingProfiles;

namespace E_commerceApplication.Extentions;

public static class CoreServiceExtensions
{

    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        // services.AddControllers();
        services.AddScoped<IServiceManager, ServiceManager>();
        services.AddAutoMapper(cfg => { }, typeof(ProductProfile).Assembly);
        return services;
    }
}