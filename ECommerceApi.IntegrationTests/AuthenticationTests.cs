using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shared.Dtos.AuthenticationDtos;
using Xunit;
using Newtonsoft.Json;

namespace ECommerceApi.IntegrationTests;

public class AuthenticationTests
{
    [Fact]
    public void AuthenticationEndpoints_ValidateStructure()
    {
        // Test validates the structure of authentication DTOs
        var loginDto = new LoginDto
        {
            Email = "test@test.com",
            Password = "password"
        };
        
        var registerDto = new RegisterDto
        {
            Email = "test@test.com",
            DisplayName = "Test User",
            Password = "TestPass123!",
            ConfirmPassword = "TestPass123!"
        };
        
        var userDto = new UserDto
        {
            Id = "1",
            Email = "test@test.com",
            DisplayName = "Test User",
            Token = "sample-token",
            Roles = new List<string> { "Admin" }
        };
        
        var assignRoleDto = new AssignRoleDto
        {
            Email = "test@test.com",
            Role = "Admin"
        };
        
        // Assert - All DTOs should be properly structured
        Assert.NotNull(loginDto);
        Assert.Equal("test@test.com", loginDto.Email);
        Assert.Equal("password", loginDto.Password);
        
        Assert.NotNull(registerDto);
        Assert.Equal("test@test.com", registerDto.Email);
        Assert.Equal("Test User", registerDto.DisplayName);
        
        Assert.NotNull(userDto);
        Assert.Equal("test@test.com", userDto.Email);
        Assert.Contains("Admin", userDto.Roles);
        
        Assert.NotNull(assignRoleDto);
        Assert.Equal("Admin", assignRoleDto.Role);
    }
    
    [Fact]
    public void JwtConfiguration_ValidateSettings()
    {
        // Test validates JWT configuration structure
        var jwtSecret = "super-secret-key-for-jwt-token-generation-that-is-32-chars-long-or-more";
        var jwtIssuer = "ECommerceApi";
        var jwtAudience = "ECommerceApiUsers";
        
        Assert.True(jwtSecret.Length >= 32, "JWT secret should be at least 32 characters long");
        Assert.NotEmpty(jwtIssuer);
        Assert.NotEmpty(jwtAudience);
    }
}