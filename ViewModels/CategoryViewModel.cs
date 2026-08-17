using System.ComponentModel.DataAnnotations;

namespace ECommerce_Mvc.ViewModels;

public class CategoryViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
}