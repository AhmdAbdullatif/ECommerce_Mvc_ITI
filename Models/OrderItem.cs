namespace ECommerce_Mvc.Models;

public class OrderItem
{
    private OrderItem() { } // Required by EF Core

    public int OrderId { get; private set; } // is set by the database when associating with an order
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    public Product? Product { get; private set; }
    public Order? Order { get; private set; }
    public OrderItem(int productId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

}
