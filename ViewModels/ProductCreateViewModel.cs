using System.ComponentModel.DataAnnotations;

namespace ECommerce_Mvc.ViewModels;

public class ProductCreateViewModel
{
    [Required]
    [StringLength(75)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public string PictureUri { get; set; } = string.Empty;
}