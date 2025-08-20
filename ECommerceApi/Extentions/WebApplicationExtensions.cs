using Domain.Contracts;

namespace E_commerceApplication.Extentions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> DbSeedingAsync(this WebApplication app)
    {
        
        using var scope = app.Services.CreateScope();
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        await dbInitializer.InitializeDbAsync();
        await dbInitializer.IdentitySeedAsync();
        return app;
    }
}