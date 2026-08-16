using ECommerce_Mvc.Enums;

namespace ECommerce_Mvc.Models
{
    public class SellerRequest
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public SellerRequestStatus Status { get; set; }
            = SellerRequestStatus.Pending;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReviewedAt { get; set; }

        public string? ReviewedBy { get; set; }

        // Navigation Property
        public ApplicationUser User { get; set; } = null!;
    }
}
