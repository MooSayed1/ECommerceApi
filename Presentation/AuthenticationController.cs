using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Interfaces;
using Shared.Dtos.OrderDtos;
using Shared.Dtos.User;

namespace Presintation;

public class AuthenticationController(IServiceManager serviceManager) : ApiController
{
    [HttpPost("Login")]
    public async Task<ActionResult<UserResultDto>> Login([FromBody] LoginDto loginDto)
    {
        var result = await serviceManager.AuthenticationService.LoginAsync(loginDto);
        return Ok(result);
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserResultDto>> Register([FromBody] RegisterDto dto)
    {
        var result = await serviceManager.AuthenticationService.RegisterAsync(dto);
        return Ok(result);
    }

    [HttpGet("EmailExist")]
    public async Task<ActionResult<bool>> CheckEmailExist(string email)
    {
        return Ok(await serviceManager.AuthenticationService.CheckIfUserExist(email));
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserResultDto>> GetCurrentUser()
    {
        return Ok(await serviceManager.AuthenticationService.GetUserByEmail(User.FindFirstValue(ClaimTypes.Email)!));
    }

    [HttpGet("Address")]
    [Authorize]
    public async Task<ActionResult<AddressDto>> GetAddress()
    {
        return Ok(await serviceManager.AuthenticationService.GetUserAddress(User.FindFirstValue(ClaimTypes.Email)!));
    }

    [HttpPut("Address")]
    [Authorize]
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto dto)
    {
        return Ok(await serviceManager.AuthenticationService.UpdateUserAddress(User.FindFirstValue(ClaimTypes.Email)!, dto));
    }
    
}