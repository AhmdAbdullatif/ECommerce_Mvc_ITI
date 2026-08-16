namespace ECommerce_Mvc.Models;

public class CartItem
{
    private CartItem() { } // Required by EF Core

    public int ProductId { get; private set; }
    public int CartId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    public Product? Product { get; private set; }
    public Cart? Cart { get; private set; }

    public CartItem(int productId, int cartId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        CartId = cartId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

}
