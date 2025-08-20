using Shared.Dtos.User;

namespace Services.Abstraction;

public interface IAuthenticationService
{
    // email , DisplayName, Token
    public Task<UserResultDto>LoginAsync(LoginDto loginDto);
    public Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
}