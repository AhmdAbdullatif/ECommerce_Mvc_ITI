using System.Security.Claims;
using ECommerce_Mvc.Constants;
using ECommerce_Mvc.Data;
using ECommerce_Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Mvc.Controllers;

[Authorize(Roles = AuthorizationConstants.SELLERS)]
public class SellerDashboardController : Controller
{
    private readonly AppDbContext _context;

    public SellerDashboardController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Get the logged-in seller ID
        var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (sellerId == null)
        {
            return Unauthorized();
        }

        // Number of products owned by this seller
        var productsCount = await _context.Products
            .CountAsync(p => p.UserId == sellerId);

        // Number of orders containing this seller's products
        var ordersCount = await _context.OrderItems
            .Where(oi => oi.Product != null &&
                         oi.Product.UserId == sellerId)
            .Select(oi => oi.OrderId)
            .Distinct()
            .CountAsync();

        // Total sales of this seller
        var sales = await _context.OrderItems
            .Where(oi => oi.Product != null &&
                         oi.Product.UserId == sellerId)
            .SumAsync(oi => oi.Quantity * oi.UnitPrice);

        var model = new SellerDashboardViewModel
        {
            ProductsCount = productsCount,
            OrdersCount = ordersCount,
            Sales = sales
        };

        return View(model);
    }
}