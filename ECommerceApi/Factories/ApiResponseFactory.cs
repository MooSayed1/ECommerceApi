using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_commerceApplication.Factories;

public class ApiResponseFactory
{
    public static IActionResult CustomValidationErrorResponse(ActionContext context)
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Any())
            .Select(e => new ValidationErrorResponse.ValidationError()
            {
                Field = e.Key,
                Errors = e.Value.Errors.Select(err => err.ErrorMessage).ToArray()
            });

        var response = new ValidationErrorResponse
        {
            StatusCode = 400,
            ErrorMessage = "Validation Failed",
            Errors = errors
        };

        return new BadRequestObjectResult(response);
    }
}