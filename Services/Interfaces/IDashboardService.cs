using ECommerce_Mvc.ViewModels;

namespace ECommerce_Mvc.Services.Interfaces;

public interface IDashboardService
{
    Task<AdminDashboardVM> GetAdminDashboardAsync();
    Task<SellerDashboardVM> GetSellerDashboardAsync(string sellerId);
}