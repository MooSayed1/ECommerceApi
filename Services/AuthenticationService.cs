using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction;
using Shared.Dtos.User;
using ValidationException = Domain.Exceptions.ValidationException;

namespace Services;

public class AuthenticationService(UserManager<User> userManager,IConfiguration configuration) : IAuthenticationService
{
    public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);

        if (user == null)
            throw new UnAuthorizedException();

        var isPasswordValid = await userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordValid)
            throw new UnAuthorizedException();

        return new UserResultDto(user.DisplayName,await CreateTokenAsync(user), user.Email);
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

    public async Task<string> CreateTokenAsync(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.DisplayName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        // d8336c239e1122e51b5a0a34d6968efb376134171fba9a697eab85efab82a16f003eaaa9f9a1dfc8f400197a991b8dce
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JwtOptions:Key"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer:configuration["JwtOptions:Issuer"],audience:configuration["JwtOptions:Audience"],claims:claims,expires:DateTime.Now.AddDays(Convert.ToDouble(configuration["JwtOptions:ExpireDate"])),signingCredentials:creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}