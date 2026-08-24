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
    public string SellerId { get; set; } = string.Empty;
    public Category? Category { get; private set; }
    public string UserId { get; private set; } = null!;
    public ApplicationUser? User { get; private set; }

   
    public Product(int categoryId,
        string name,
        string description,
        int quantity,
        decimal price,
        string pictureUri,
        string userId)
    {
        CategoryId = categoryId;
        Name = name;
        Description = description;
        Quantity = quantity;
        Price = price;
        PictureUri = pictureUri;
        UserId = userId;
    }
}
