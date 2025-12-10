
using Management.Domain.Shared.Constants;  
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Management.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

        foreach (var role in new[] { SystemRoles.Administrator, SystemRoles.Employee })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }
    }

    public static async Task SeedAdminAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var adminEmail = configuration["ADMIN_EMAIL"] ?? "admin@management.com";
        var adminPassword = configuration["ADMIN_PASSWORD"] ?? "Admin123";

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin != null)
        {
            return;
        }

        var newAdmin = new ApplicationUser
        {
            FirstName = "System",
            LastName = "Administrator",
            UserName = adminEmail,
            Email = adminEmail,
            DocumentNumber = "0000000000",
            RegisterDate = DateTime.UtcNow,
            EmailConfirmed = true
        };

        var creationResult = await userManager.CreateAsync(newAdmin, adminPassword);
        if (!creationResult.Succeeded)
        {
            var errors = string.Join(", ", creationResult.Errors.Select(error => error.Description));
            throw new InvalidOperationException($"Failed to create default admin user: {errors}");
        }

        if (!await roleManager.RoleExistsAsync(SystemRoles.Administrator))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(SystemRoles.Administrator));
        }

        await userManager.AddToRoleAsync(newAdmin, SystemRoles.Administrator);
    }
}
