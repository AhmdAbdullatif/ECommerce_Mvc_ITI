using Microsoft.AspNetCore.Identity;

namespace ECommerce_Mvc.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<SellerRequest> SellerRequests { get; set; } = new List<SellerRequest>();
}
