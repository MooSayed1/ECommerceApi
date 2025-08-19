using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.Identity;

namespace Persistance.Identity.DataSeeding;

public class IdentityDbInitializer : IIdentityDbInitializer
{
    private readonly IdentityAppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityDbInitializer(
        IdentityAppDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Migrate database if pending migrations exist
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
            {
                await _context.Database.MigrateAsync();
            }

            // Create roles if they don't exist
            await CreateRoles();
            
            // Create admin users if they don't exist
            await CreateAdminUsers();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during Identity DB initialization: {ex.Message}");
        }
    }

    private async Task CreateRoles()
    {
        var roles = new[] { "Admin", "SuperAdmin" };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private async Task CreateAdminUsers()
    {
        // Create Super Admin User
        var superAdminEmail = "superadmin@test.com";
        var superAdminUser = await _userManager.FindByEmailAsync(superAdminEmail);
        
        if (superAdminUser == null)
        {
            superAdminUser = new User
            {
                UserName = superAdminEmail,
                Email = superAdminEmail,
                DisplayName = "Super Administrator",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(superAdminUser, "SuperAdmin123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(superAdminUser, "Admin");
            }
        }

        // Create Admin User
        var adminEmail = "admin@test.com";
        var adminUser = await _userManager.FindByEmailAsync(adminEmail);
        
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                DisplayName = "Administrator",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}