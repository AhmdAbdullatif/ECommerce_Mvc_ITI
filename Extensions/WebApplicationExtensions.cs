using ECommerce_Mvc.Data;
using ECommerce_Mvc.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerce_Mvc.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var sellerId = await IdentityContextSeed.SeedAsync(userManager, roleManager, app.Logger);

        await ProductContextSeed.SeedAsync(sellerId, dbContext, app.Logger, 3);
    }

}
