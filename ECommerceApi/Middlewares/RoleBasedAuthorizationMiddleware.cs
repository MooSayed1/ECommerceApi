using System.Security.Claims;

namespace E_commerceApplication.Middlewares;

public class RoleBasedAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RoleBasedAuthorizationMiddleware> _logger;

    public RoleBasedAuthorizationMiddleware(RequestDelegate next, ILogger<RoleBasedAuthorizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Let authentication middleware handle JWT validation first
        await _next(context);
        
        // This middleware is more for logging and monitoring role access
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = context.User.FindFirst(ClaimTypes.Email)?.Value;
            var userRoles = context.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            
            if (userRoles.Any())
            {
                _logger.LogInformation($"User {userEmail} (ID: {userId}) accessed {context.Request.Path} with roles: {string.Join(", ", userRoles)}");
            }
        }
    }
}