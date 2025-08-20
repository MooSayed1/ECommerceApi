using Shared.Dtos.User;
using Domain.Entities;  // Keep this import from dev branch

namespace Services.Abstraction;

public interface IAuthenticationService
{
    // email , DisplayName, Token
    public Task<UserResultDto>LoginAsync(LoginDto loginDto);
    public Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
    public Task<string> CreateTokenAsync(User user);  // Keep this method from dev branch
}
