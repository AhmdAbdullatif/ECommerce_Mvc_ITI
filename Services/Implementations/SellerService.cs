using ECommerce_Mvc.Data;
using ECommerce_Mvc.Enums;
using ECommerce_Mvc.Models;
using ECommerce_Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace ECommerce_Mvc.Services.Implementations
{
    public class SellerService : ISellerService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SellerService(
            AppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<bool> RequestToBecomeSellerAsync(
          string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            var existingRequest = await _context.SellerRequests
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.Status == SellerRequestStatus.Pending);

            if (existingRequest)
                return false;

            var isSeller = await _userManager.IsInRoleAsync(
                user,
                "Seller");

            if (isSeller)
                return false;

            var request = new SellerRequest
            {
                UserId = userId,
                Status = SellerRequestStatus.Pending,
                RequestedAt = DateTime.UtcNow
            };

            await _context.SellerRequests.AddAsync(request);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ApproveSellerAsync(
            int requestId,
            string adminId)
        {
            var request = await _context.SellerRequests
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == requestId);

            if (request == null)
                return false;

            if (request.Status != SellerRequestStatus.Pending)
                return false;

            var result = await _userManager.AddToRoleAsync(
                request.User,
                "Seller");

            if (!result.Succeeded)
                return false;

            request.Status = SellerRequestStatus.Approved;
            request.ReviewedAt = DateTime.UtcNow;
            request.ReviewedBy = adminId;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectSellerAsync(
           int requestId,
           string adminId)
        {
            var request = await _context.SellerRequests
                .FirstOrDefaultAsync(x => x.Id == requestId);

            if (request == null)
                return false;

            if (request.Status != SellerRequestStatus.Pending)
                return false;

            request.Status = SellerRequestStatus.Rejected;
            request.ReviewedAt = DateTime.UtcNow;
            request.ReviewedBy = adminId;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<SellerRequest>> GetPendingRequestsAsync()
        {
            return await _context.SellerRequests
                .Include(x => x.User)
                .Where(x =>
                    x.Status == SellerRequestStatus.Pending)
                .ToListAsync();
        }

    }
}
