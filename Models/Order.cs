namespace ECommerce_Mvc.Models;

public class Order
{
#pragma warning disable CS8618
    private Order() { } // Required by EF Core

    private readonly List<OrderItem> _orderedItems = new();

    public int Id { get; private set; }
    public Address ShipToAddress { get; private set; }
    public DateTime OrderDateUtc { get; private set; } = DateTime.UtcNow;
    public IReadOnlyCollection<OrderItem> OrderedItems => _orderedItems.AsReadOnly();
    public string UserId { get; private set; }
    public ApplicationUser? User { get; private set; }

    public Order(string userId, Address shipToAddress, List<OrderItem> items)
    {
        UserId = userId;
        ShipToAddress = shipToAddress;
        _orderedItems = items;
    }

}
