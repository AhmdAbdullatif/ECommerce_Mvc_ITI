namespace ECommerce_Mvc.Models;

public class Cart
{
    private Cart() { } // Required by EF Core
    private readonly List<CartItem> _items = [];

    public int Id { get; private set; }
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
    public string BuyerId { get; private set; } = null!;
    public int TotalItems => _items.Sum(x => x.Quantity);

    public Cart(string buyerId)
    {
        BuyerId = buyerId;
    }
}
