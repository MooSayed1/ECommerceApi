using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Exceptions;
using Shared.ErrorModels;

namespace E_commerceApplication.Middlewares;

public class GlobalHandlingMiddleware(RequestDelegate next, ILogger<GlobalHandlingMiddleware> logger)
{
    // Response [statusCode,ErrorMsg]
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next.Invoke(httpContext);
            // if there's no exceptions 
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                await HandelNotFoundAsync(httpContext);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
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
        
        var response = new ErrorDetails()
        {
            ErrorMessage = ex.Message,
        };
        
        httpContext.Response.StatusCode = ex switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ValidationException validationException => HandelValidationAsync(response, validationException),
            _ => StatusCodes.Status500InternalServerError,
        };

        await httpContext.Response.WriteAsync(response.ToString()); // ToString convert to Json
    }

    private int HandelValidationAsync(ErrorDetails response , ValidationException validationException)
    {
        response.Errors = validationException.Errors;
        response.StatusCode = StatusCodes.Status400BadRequest;
        return StatusCodes.Status400BadRequest;
    }
}