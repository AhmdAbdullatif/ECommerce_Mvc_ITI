using ECommerce_Mvc.Data;
using ECommerce_Mvc.Models;
using ECommerce_Mvc.Repositories.Interfaces;
using ECommerce_Mvc.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Mvc.Repositories.Implementations;

public class CartRepository(AppDbContext context) : ICartRepository
{
    public async Task<Cart?> GetByBuyerIdAsync(string buyerId)
    {
        return await context.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
    }
    
    public async Task<CartViewModel?> GetByBuyerIdWithItemsAsync(string buyerId)
    {
        return await context.Carts
            .AsNoTracking()
            .Where(x => x.BuyerId == buyerId)
            .Select(x => new CartViewModel()
            {
                 CartId = x.Id,
                  CartItems = x.Items.Select(ci => new CartItemViewModel()
                  {
                       Quantity = ci.Quantity,
                       UnitPrice = ci.UnitPrice,
                       ProductId = ci.ProductId,
                       ProductName = ci.Product!.Name,
                       ProductDescription = ci.Product.Description,
                       PictureUri = ci.Product.PictureUri
                  })
            }).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Cart cart)
    {
        await context.AddAsync(cart);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
