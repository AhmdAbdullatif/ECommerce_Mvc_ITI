using ECommerce_Mvc.Constants;
using ECommerce_Mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Mvc.Data;

public class ProductContextSeed
{
    public static async Task SeedAsync(AppDbContext context,
        UserManager<ApplicationUser> userManager,
        ILogger logger,
        int retry = 0)
    {
        int retryForAvailability = retry;
        string sellerId;
        try
        {
            List<Category> categories;
            if (!await context.Categories.AnyAsync())
            {
                categories = PreconfiguredProductCategories().ToList();
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
            else
            {
                categories = await context.Categories.ToListAsync();
            }

            var sellerEmail = "seller@example.com";
            var seller = await userManager.FindByEmailAsync(sellerEmail);
            if (seller is null)
            {
                seller = new ApplicationUser()
                {
                    Email = sellerEmail,
                    UserName = sellerEmail
                };

                await userManager.CreateAsync(seller, AuthorizationConstants.DEFAULT_PASSWORD);
                await userManager.AddToRoleAsync(seller, AuthorizationConstants.SELLERS);
            }
            sellerId = seller.Id;

            if (!await context.Products.AnyAsync())
            {
                var products = PreconfiguredProducts(sellerId, categories);
                await context.Products.AddRangeAsync(products);
            }

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10) throw;

            retryForAvailability++;

            logger.LogError(ex.Message);
            await SeedAsync(context, userManager, logger, retryForAvailability);
        }
    }

    private static IEnumerable<Category> PreconfiguredProductCategories()
    {
        return
        [
            new Category("Shoes"),
            new Category("Clothing"),
            new Category("Accessories"),
            new Category("Electronics"),
            new Category("Home & Garden")
        ];
    }

    private static List<Product> PreconfiguredProducts(string sellerId,
        IEnumerable<Category> categories)
    {
        var categoryList = categories.ToList();

        var shoes = categoryList.First(c => c.Name == "Shoes").Id;
        var clothing = categoryList.First(c => c.Name == "Clothing").Id;
        var accessories = categoryList.First(c => c.Name == "Accessories").Id;
        var electronics = categoryList.First(c => c.Name == "Electronics").Id;
        var homeGarden = categoryList.First(c => c.Name == "Home & Garden").Id;

        return
        [
            new Product(shoes, "Air Max 90",
                "Classic Nike running shoes with visible Air cushioning",
                50, 129.99m,
                "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=800",
                sellerId),

            new Product(shoes, "Ultraboost 22",
                "High-performance running shoes with Boost midsole",
                40, 189.99m,
                "https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?w=800",
                sellerId),

            new Product(shoes, "Suede Classic",
                "Iconic casual sneakers with suede upper",
                60, 79.99m,
                "https://images.unsplash.com/photo-1608231387042-66d1773070a5?w=800",
                sellerId),

            new Product(clothing, "Dri-FIT T-Shirt",
                "Moisture-wicking training shirt",
                100, 34.99m,
                "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=800",
                sellerId),

            new Product(clothing, "Essentials Hoodie",
                "Comfortable everyday hoodie",
                80, 59.99m,
                "https://images.unsplash.com/photo-1556821840-3a63f95609a7?w=800",
                sellerId),

            new Product(accessories, "Sports Cap",
                "Adjustable sports cap",
                120, 24.99m,
                "https://images.unsplash.com/photo-1588850561407-ed78c456fe18?w=800",
                sellerId),

            new Product(electronics, "iPhone 15",
                "Latest smartphone with advanced camera system",
                30, 999.00m,
                "https://images.unsplash.com/photo-1695048133142-1a20484d2569?w=800",
                sellerId),

            new Product(electronics, "Galaxy S24",
                "Flagship Android smartphone",
                25, 899.00m,
                "https://images.unsplash.com/photo-1610945265064-0e34e5519bbf?w=800",
                sellerId),

            new Product(homeGarden, "Billy Bookcase",
                "Classic bookshelf unit",
                15, 79.00m,
                "https://images.unsplash.com/photo-1594620302200-9a762244a156?w=800",
                sellerId),

            new Product(homeGarden, "Klippan Sofa",
                "Compact two-seat sofa",
                10, 249.00m,
                "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?w=800",
                sellerId)
        ];
    }
}