namespace ECommerce_Mvc.Models;

public class Review
{
    private Review() { }

    public int Id { get; private set; }

    public int ProductId { get; private set; }
    public Product? Product { get; private set; }

    public string UserId { get; private set; } = null!;
    public ApplicationUser? User { get; private set; }

    public int Rating { get; private set; }

    public string Comment { get; private set; } = null!;

    public Review(
        int productId,
        string userId,
        int rating,
        string comment)
    {
        ProductId = productId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
    }
}