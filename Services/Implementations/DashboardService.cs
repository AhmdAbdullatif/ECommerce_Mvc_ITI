using ECommerce_Mvc.Data;
using ECommerce_Mvc.Models;
using ECommerce_Mvc.Services.Interfaces;
using ECommerce_Mvc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Mvc.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardService(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<AdminDashboardVM> GetAdminDashboardAsync()
    {
        var customers = await _userManager.GetUsersInRoleAsync("Customer");
        var sellers = await _userManager.GetUsersInRoleAsync("Seller");

        var dashboard = new AdminDashboardVM
        {
            TotalCustomers = customers.Count,
            TotalSellers = sellers.Count,
            TotalProducts = await _context.Products.CountAsync(),
            TotalOrders = await _context.Orders.CountAsync(),
            PendingOrders = 0
        };

        return dashboard;
    }

    public async Task<SellerDashboardVM> GetSellerDashboardAsync(string sellerId)
    {
        var productsCount = await _context.Products
            .CountAsync(p => p.UserId == sellerId);

        var sellerProductIds = await _context.Products
            .Where(p => p.UserId == sellerId)
            .Select(p => p.Id)
            .ToListAsync();

        var sellerOrderItems = await _context.OrderItems
            .Where(oi => sellerProductIds.Contains(oi.ProductId))
            .ToListAsync();

        var ordersCount = await _context.OrderItems
            .Where(oi => sellerProductIds.Contains(oi.ProductId))
            .Select(oi => oi.OrderId)
            .Distinct()
            .CountAsync();

        var sales = sellerOrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

        return new SellerDashboardVM
        {
            ProductsCount = productsCount,
            OrdersCount = ordersCount,
            Sales = sales
        };
    }
}