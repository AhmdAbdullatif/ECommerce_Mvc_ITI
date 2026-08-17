using System.ComponentModel.DataAnnotations;

namespace ECommerce_Mvc.Requests;

public class AddItemToCartRequest
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }
}
