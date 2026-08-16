using ECommerce_Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Mvc.Data;

public class ProductContextSeed
{
    public static async Task SeedAsync(
        AppDbContext context,
        ILogger logger,
        int retry = 0)
    {
        int retryForAvailability = retry;

        try
        {
            // 1. Make sure the required categories exist
            var requiredCategories =
                PreconfiguredProductCategories();

            var existingCategoryNames =
                await context.Categories
                    .Select(c => c.Name)
                    .ToListAsync();

            var missingCategories =
                requiredCategories
                    .Where(c =>
                        !existingCategoryNames.Contains(c.Name))
                    .ToList();

            if (missingCategories.Count > 0)
            {
                await context.Categories
                    .AddRangeAsync(missingCategories);

                await context.SaveChangesAsync();
            }

            // 2. Read the real categories from the database
            var categories =
                await context.Categories
                    .ToListAsync();

            // 3. Get the real seller created by Identity seed
            var seller =
                await context.Users
                    .FirstOrDefaultAsync(
                        u => u.UserName == "seller@example.com");

            if (seller is null)
            {
                throw new InvalidOperationException(
                    "Seller user was not found. Identity seed must run before product seed.");
            }

            // 4. Seed products only if the table is empty
            if (!await context.Products.AnyAsync())
            {
                var products =
                    PreconfiguredProducts(
                        categories,
                        seller.Id);

                await context.Products
                    .AddRangeAsync(products);

                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10)
            {
                throw;
            }

            retryForAvailability++;

            logger.LogError(
                ex,
                "Error while seeding products. Retry: {Retry}",
                retryForAvailability);

            await SeedAsync(
                context,
                logger,
                retryForAvailability);
        }
    }

    private static List<Category>
        PreconfiguredProductCategories()
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

    private static List<Product> PreconfiguredProducts(
        IEnumerable<Category> categories,
        string sellerId)
    {
        var categoryList = categories.ToList();

        var shoes =
            categoryList.First(
                c => c.Name == "Shoes").Id;

        var clothing =
            categoryList.First(
                c => c.Name == "Clothing").Id;

        var accessories =
            categoryList.First(
                c => c.Name == "Accessories").Id;

        var electronics =
            categoryList.First(
                c => c.Name == "Electronics").Id;

        var homeGarden =
            categoryList.First(
                c => c.Name == "Home & Garden").Id;

        return
        [
            new Product(
                shoes,
                "Air Max 90",
                "Classic Nike running shoes with visible Air cushioning",
                50,
                129.99m,
                "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=800",
                sellerId),

            new Product(
                shoes,
                "Ultraboost 22",
                "High-performance running shoes with Boost midsole",
                40,
                189.99m,
                "https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?w=800",
                sellerId),

            new Product(
                shoes,
                "Suede Classic",
                "Iconic casual sneakers with suede upper",
                60,
                79.99m,
                "https://images.unsplash.com/photo-1608231387042-66d1773070a5?w=800",
                sellerId),

            new Product(
                clothing,
                "Dri-FIT T-Shirt",
                "Moisture-wicking training shirt",
                100,
                34.99m,
                "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=800",
                sellerId),

            new Product(
                clothing,
                "Essentials Hoodie",
                "Comfortable everyday hoodie",
                80,
                59.99m,
                "https://images.unsplash.com/photo-1556821840-3a63f95609a7?w=800",
                sellerId),

            new Product(
                accessories,
                "Sports Cap",
                "Adjustable sports cap",
                120,
                24.99m,
                "https://images.unsplash.com/photo-1588850561407-ed78c456fe18?w=800",
                sellerId),

            new Product(
                electronics,
                "iPhone 15",
                "Latest smartphone with advanced camera system",
                30,
                999.00m,
                "https://images.unsplash.com/photo-1695048133142-1a20484d2569?w=800",
                sellerId),

            new Product(
                electronics,
                "Galaxy S24",
                "Flagship Android smartphone",
                25,
                899.00m,
                "https://images.unsplash.com/photo-1610945265064-0e34e5519bbf?w=800",
                sellerId),

            new Product(
                homeGarden,
                "Billy Bookcase",
                "Classic bookshelf unit",
                15,
                79.00m,
                "https://images.unsplash.com/photo-1594620302200-9a762244a156?w=800",
                sellerId),

            new Product(
                homeGarden,
                "Klippan Sofa",
                "Compact two-seat sofa",
                10,
                249.00m,
                "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?w=800",
                sellerId)
        ];
    }
}