using Shared.Dtos.AuthenticationDtos;

namespace Services.Abstraction.Interfaces;

public interface IAuthService
{
    Task<UserDto?> LoginAsync(LoginDto loginDto);
    Task<UserDto?> RegisterAsync(RegisterDto registerDto);
    Task<UserDto?> GetCurrentUserAsync(string userId);
    Task<bool> AssignRoleAsync(AssignRoleDto assignRoleDto);
    Task<bool> EmailExistsAsync(string email);
    Task<IList<string>> GetUserRolesAsync(string userId);
}