using ECommerce_Mvc.Constants;
using ECommerce_Mvc.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerce_Mvc.Data;

public static class IdentityContextSeed
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger logger)
    {
        try
        {
            await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.ADMINISTRATORS));
            await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.SELLERS));

            var defaultUser = new ApplicationUser { UserName = "user@exmaple.com", Email = "user@example.com" };
            await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);

            var seller = new ApplicationUser { UserName = "seller@example.com", Email = "seller@example.com" };
            await userManager.CreateAsync(seller, AuthorizationConstants.DEFAULT_PASSWORD);
            seller = await userManager.FindByNameAsync(seller.UserName);
            if (seller != null)
            {
                await userManager.AddToRoleAsync(seller, AuthorizationConstants.SELLERS);
            }

            string adminUserName = "admin@example.com";
            var adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName };
            await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
            adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser != null)
            {
                await userManager.AddToRoleAsync(adminUser, AuthorizationConstants.ADMINISTRATORS);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database with Identity users");
        }
    }

}
