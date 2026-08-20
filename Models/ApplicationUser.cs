using Microsoft.AspNetCore.Identity;

namespace ECommerce_Mvc.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<SellerRequest> SellerRequests { get; set; } = new List<SellerRequest>();

}
