using ECommerce_Mvc.Models;
using ECommerce_Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Mvc.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardController(
        IDashboardService dashboardService,
        UserManager<ApplicationUser> userManager)
    {
        _dashboardService = dashboardService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Admin()
    {
        var dashboard = await _dashboardService.GetAdminDashboardAsync();

        return View(dashboard);
    }

    public async Task<IActionResult> Seller()
    {
        var sellerId = _userManager.GetUserId(User);

        if (sellerId == null)
        {
            return Unauthorized();
        }

        var dashboard = await _dashboardService.GetSellerDashboardAsync(sellerId);

        return View(dashboard);
    }
}