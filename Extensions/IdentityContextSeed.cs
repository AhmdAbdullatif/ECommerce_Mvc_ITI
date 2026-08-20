using Microsoft.AspNetCore.Identity;

namespace ECommerce_Mvc.Extensions;

public static class IdentityContextSeed
{
    public static async Task SeedAsync(
        UserManager<Models.ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger logger)
    {
        string[] roles =
        {
            "Admin",
            "Seller",
            "Customer"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}