using Domain.Contracts;

namespace E_commerceApplication.Extentions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> DbSeedingAsync(this WebApplication app)
    {
        
        using var scope = app.Services.CreateScope();
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        var identityDbInitializer = scope.ServiceProvider.GetRequiredService<IIdentityDbInitializer>();
        
        await dbInitializer.InitializeDbAsync();
        await identityDbInitializer.InitializeAsync();
        
        return app;
    }
}