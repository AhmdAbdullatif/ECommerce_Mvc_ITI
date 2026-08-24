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

    public void AddItem(int productId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity cannot be zero or negative");

        if (unitPrice <= 0)
            throw new ArgumentException("Unit price cannot be zero or negative");

        var existingItem = _items.FirstOrDefault(x => x.ProductId == productId);
        if (existingItem is null)
        {
            _items.Add(new CartItem(productId, Id, quantity, unitPrice));
            return;
        }
        else
        {
            existingItem.AddQuantity(quantity);
        }
    }

    public void RemoveItem(int productId)
    {
        var item = _items.FirstOrDefault(x => x.ProductId == productId);
        if (item == null)
            return;

        _items.Remove(item);
    }

    public void UpdateItemQuantity(int productId, int quantity)
    {
        var item = _items.FirstOrDefault(x => x.ProductId == productId)
            ?? throw new ArgumentException($"Product with ID: {productId} is not in the cart.");

        item.UpdateQuantity(quantity);
    }
}
