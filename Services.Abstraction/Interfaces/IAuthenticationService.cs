using Domain.Entities;
using Shared.Dtos.User;

namespace Services.Abstraction.Interfaces;

public interface IAuthenticationService
{
    // email , DisplayName, Token
    public Task<UserResultDto>LoginAsync(LoginDto loginDto);
    public Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
    public Task<string> CreateTokenAsync(User user);
}
