using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction;
using Services.Abstraction.Interfaces;
using Shared.Dtos.OrderDtos;
using Shared.Dtos.User;
using ValidationException = Domain.Exceptions.ValidationException;

namespace Services;

public class AuthenticationService(UserManager<User> userManager, IConfiguration configuration, IMapper mapper)
    : IAuthenticationService
{
    public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);

        if (user == null)
            throw new UnAuthorizedException();

        var isPasswordValid = await userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordValid)
            throw new UnAuthorizedException();

        return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
    }

    public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
    {
        var user = new User()
        {
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName,
            PhoneNumber = registerDto.PhoneNumber,
            UserName = registerDto.UserName
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);

        var errors = result.Errors.Select(e => e.Description).ToList();

        throw new ValidationException(errors);
    }

    public async Task<bool> CheckIfUserExist(string email)
    {
        return await userManager.FindByEmailAsync(email) != null;
    }

    public async Task<AddressDto> GetUserAddress(string email)
    {
        var user = await userManager.Users.Include(o => o.Address).FirstOrDefaultAsync(o => o.Email == email)
                   ?? throw new UserNotFoundException(email);
        return mapper.Map<AddressDto>(user.Address);
    }

    public async Task<UserResultDto> GetUserByEmail(string email)
    {
        var user = await userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException(email);
        return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
    }

    public async Task<AddressDto> UpdateUserAddress(string email, AddressDto addressDto)
    {
        var user = await userManager.Users.Include(o => o.Address).FirstOrDefaultAsync(o => o.Email == email)
                   ?? throw new UserNotFoundException(email);
        if (user?.Address != null)
        {
            user.Address.FirstName = addressDto.FirstName;
            user.Address.LastName = addressDto.LastName;
            user.Address.City = addressDto.City;
            user.Address.Country = addressDto.Country;
            user.Address.Street = addressDto.Street;
        }

        user!.Address = mapper.Map<Address>(addressDto);
        await userManager.UpdateAsync(user);
        return  mapper.Map<AddressDto>(user.Address);
    }

    private async Task<string> CreateTokenAsync(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.DisplayName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: configuration["JwtOptions:Issuer"],
            audience: configuration["JwtOptions:Audience"], claims: claims,
            expires: DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["JwtOptions:ExpireDate"])),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}