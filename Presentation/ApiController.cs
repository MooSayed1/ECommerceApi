using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace Presintation;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(ValidationErrorResponse),StatusCodes.Status400BadRequest)]
public class ApiController : ControllerBase
{
}