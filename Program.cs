using ECommerce_Mvc.Data;
using ECommerce_Mvc.Extensions;
using ECommerce_Mvc.Models;
using ECommerce_Mvc.Repositories.Implementations;
using ECommerce_Mvc.Repositories.Interfaces;
using ECommerce_Mvc.Services.Implementations;
using ECommerce_Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
// Identity


builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();


builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IProductService, ECommerce_Mvc.Services.Implementations.ProductService>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAccountService, ECommerce_Mvc.Services.Implementations.AccountService>();

builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<IAnonymousCartManager, AnonymousCartManager>();



var app = builder.Build();

await app.SeedDatabaseAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

    await IdentitySeeder.SeedRolesAsync(roleManager);
}

app.Run();
