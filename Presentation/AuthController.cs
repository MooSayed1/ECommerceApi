using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Interfaces;
using Shared.Dtos.AuthenticationDtos;
using System.Security.Claims;

namespace Presentation;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public AuthController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _serviceManager.AuthService.LoginAsync(loginDto);
        if (user == null)
            return BadRequest(new { message = "Invalid credentials" });

        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await _serviceManager.AuthService.EmailExistsAsync(registerDto.Email))
            return BadRequest(new { message = "Email is already in use" });

        var user = await _serviceManager.AuthService.RegisterAsync(registerDto);
        if (user == null)
            return BadRequest(new { message = "Failed to create user" });

        return Ok(user);
    }

    [HttpGet("current-user")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var user = await _serviceManager.AuthService.GetCurrentUserAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(user);
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult> AssignRole(AssignRoleDto assignRoleDto)
    {
        var result = await _serviceManager.AuthService.AssignRoleAsync(assignRoleDto);
        if (!result)
            return BadRequest(new { message = "Failed to assign role" });

        return Ok(new { message = "Role assigned successfully" });
    }

    [HttpGet("check-email/{email}")]
    public async Task<ActionResult<bool>> CheckEmailExists(string email)
    {
        var exists = await _serviceManager.AuthService.EmailExistsAsync(email);
        return Ok(exists);
    }

    [HttpGet("user-roles/{userId}")]
    [Authorize]
    public async Task<ActionResult<IList<string>>> GetUserRoles(string userId)
    {
        // Only allow users to check their own roles or SuperAdmin to check any user's roles
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("SuperAdmin");
        
        if (currentUserId != userId && !isAdmin)
            return Forbid();

        var roles = await _serviceManager.AuthService.GetUserRolesAsync(userId);
        return Ok(roles);
    }
}