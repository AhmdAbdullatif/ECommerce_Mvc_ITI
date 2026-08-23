using ECommerce_Mvc.Constants;
using ECommerce_Mvc.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerce_Mvc.Extensions;

public static class IdentityContextSeed
{
    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger logger)
    {
        string[] roles =
        {
            AuthorizationConstants.ADMINISTRATORS,
            AuthorizationConstants.SELLERS
        };

        // Create Roles
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult = await roleManager.CreateAsync(
                    new IdentityRole(role)
                );

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        roleResult.Errors.Select(e => e.Description)
                    );

                    throw new Exception(
                        $"Failed to create role {role}: {errors}"
                    );
                }
            }
        }

        // Create Seller User
        var sellerEmail = "seller@example.com";

        var seller = await userManager.FindByEmailAsync(sellerEmail);

        if (seller == null)
        {
            seller = new ApplicationUser
            {
                UserName = sellerEmail,
                Email = sellerEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                seller,
                AuthorizationConstants.DEFAULT_PASSWORD
            );

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description)
                );

                throw new Exception(
                    $"Failed to create seller: {errors}"
                );
            }

            var roleResult = await userManager.AddToRoleAsync(
                seller,
                AuthorizationConstants.SELLERS
            );

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    roleResult.Errors.Select(e => e.Description)
                );

                throw new Exception(
                    $"Failed to add seller to role: {errors}"
                );
            }
        }
    }
}