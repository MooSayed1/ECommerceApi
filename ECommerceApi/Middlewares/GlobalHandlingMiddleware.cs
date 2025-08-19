using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Exceptions;
using Shared.ErrorModels;

namespace E_commerceApplication.Middlewares;

public class GlobalHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalHandlingMiddleware> _logger;

    public GlobalHandlingMiddleware(RequestDelegate next, ILogger<GlobalHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    // Response [statusCode,ErrorMsg]
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandeExceptionAsync(context,ex);
        }
    }

    private async Task HandeExceptionAsync(HttpContext context,Exception ex)
    {
        context.Response.ContentType = "application/json";
        // context.Response.StatusCode = StatusCodes.Status500InternalServerError; // 500
        context.Response.StatusCode = ex switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };
        var response = new ErrorDetails()
        {
            ErrorMessage = ex.Message,
            StatusCode = context.Response.StatusCode
        };
        
        await context.Response.WriteAsync(response.ToString()); // ToString convert to Json

    }
}