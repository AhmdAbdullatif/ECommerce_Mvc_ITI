using ECommerce_Mvc.Models;

namespace ECommerce_Mvc.Models;

public class Product
{
    private Product() { } // Required by EF Core

    public int Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public string PictureUri { get; private set; } = null!;

    public int Quantity { get; private set; }

    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    public decimal Price { get; private set; }

    public int CategoryId { get; private set; }

    public string SellerId { get; private set; } = null!;

    public Category? Category { get; private set; }

    public ApplicationUser? Seller { get; private set; }

    public Product(
        int categoryId,
        string name,
        string description,
        int quantity,
        decimal price,
        string pictureUri,
        string sellerId)
    {
        CategoryId = categoryId;
        Name = name;
        Description = description;
        Quantity = quantity;
        Price = price;
        PictureUri = pictureUri;
        SellerId = sellerId;
    }
}