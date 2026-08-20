using System.ComponentModel.DataAnnotations;

namespace ECommerce_Mvc.Models.Requests;

public class AddItemToCartRequest
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(minimum: 1, maximum: int.MaxValue)]
    public int Quantity { get; set; }
}
