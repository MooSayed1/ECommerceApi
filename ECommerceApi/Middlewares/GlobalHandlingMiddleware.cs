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
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next.Invoke(httpContext);
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                await HandelNotFoundAsync(httpContext);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandeExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandelNotFoundAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = "application/json";
        var response = new ErrorDetails()
        {
            StatusCode = StatusCodes.Status404NotFound,
            ErrorMessage = $"This End Point {httpContext.Request.Path} was not found."
        };
        await httpContext.Response.WriteAsync(response.ToString());
    }

    private async Task HandeExceptionAsync(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.ContentType = "application/json";
        // context.Response.StatusCode = StatusCodes.Status500InternalServerError; // 500
        httpContext.Response.StatusCode = ex switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };
        var response = new ErrorDetails()
        {
            ErrorMessage = ex.Message,
            StatusCode = httpContext.Response.StatusCode
        };

        await httpContext.Response.WriteAsync(response.ToString()); // ToString convert to Json
    }
}