namespace ECommerce_Mvc.Models;

public class Review
{
    private Review() { } // Required by EF Core

    public int Id { get; private set; }
    public int Rating { get; private set; }        // 1 -> 5
    public string Comment { get; private set; } = null!;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    public int ProductId { get; private set; }
    public Product? Product { get; private set; }

    public string UserId { get; private set; } = null!;
    public ApplicationUser? User { get; private set; }

    public Review(int productId, string userId, int rating, string comment)
    {
        ProductId = productId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
    }
}