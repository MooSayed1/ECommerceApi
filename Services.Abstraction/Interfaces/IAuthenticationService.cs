using Domain.Entities;
using Shared.Dtos.OrderDtos;
using Shared.Dtos.User;

namespace Services.Abstraction.Interfaces;

public interface IAuthenticationService
{
    // email , DisplayName, Token
    public Task<UserResultDto> LoginAsync(LoginDto loginDto);

    public Task<UserResultDto> RegisterAsync(RegisterDto registerDto);

    // User methods
    Task<AddressDto> GetUserAddress(string email);
    Task<bool> CheckIfUserExist(string email);
    Task<UserResultDto> GetUserByEmail(string email);

    Task<AddressDto> UpdateUserAddress(string email, AddressDto addressDto);
}