using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Interfaces;
using Shared.Dtos.User;

namespace Presintation;

public class AuthenticationController : ApiController
{
    private readonly IServiceManager _serviceManager;

    public AuthenticationController(IServiceManager  serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<UserResultDto>> Login([FromBody] LoginDto loginDto)
    {
        var result = await _serviceManager.AuthenticationService.LoginAsync(loginDto);
        return Ok(result);
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserResultDto>> Register([FromBody] RegisterDto dto)
    {
        var result = await _serviceManager.AuthenticationService.RegisterAsync(dto);
        return Ok(result);
    }
}