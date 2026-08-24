using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce_Mvc.Models;
using ECommerce_Mvc.Data;
using ECommerce_Mvc.Constants;

namespace ECommerce_Mvc.Controllers
{
    [Authorize(Roles = AuthorizationConstants.ADMINISTRATORS)]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            int totalUsers = await _userManager.Users.CountAsync();
            int totalSellers = (await _userManager.GetUsersInRoleAsync(AuthorizationConstants.SELLERS)).Count;
            int totalAdmins = (await _userManager.GetUsersInRoleAsync(AuthorizationConstants.ADMINISTRATORS)).Count;

            var vm = new AdminDashboardVM
            {
                TotalCustomers = totalUsers - totalSellers - totalAdmins,
                TotalSellers = totalSellers,
                TotalProducts = await _context.Products.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                PendingOrders = 0 
            };

            return View(vm);
        }
    }
}