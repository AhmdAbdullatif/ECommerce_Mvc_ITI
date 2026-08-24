namespace ECommerce_Mvc.ViewModels;

public class CartViewModel
{
    public int CartId { get; set; }
    public IEnumerable<CartItemViewModel> CartItems { get; set; } = [];
    public decimal TotalPrice => CartItems.Sum(x => x.Quantity * x.UnitPrice);
}
