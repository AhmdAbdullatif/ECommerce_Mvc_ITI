using System.ComponentModel.DataAnnotations;

namespace ECommerce_Mvc.Models.Requests;

public class UpdateCartItemQuantityRequest
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
