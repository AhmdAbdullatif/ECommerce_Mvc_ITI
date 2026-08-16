using ECommerce_Mvc.Models;

namespace ECommerce_Mvc.Services.Interface
{
    public interface ISellerService
    {
        Task<bool> RequestToBecomeSellerAsync(string userId);

        Task<IEnumerable<SellerRequest>> GetPendingRequestsAsync();

        Task<bool> ApproveSellerAsync(
            int requestId,
            string adminId);

        Task<bool> RejectSellerAsync(
            int requestId,
            string adminId);
    }
}
