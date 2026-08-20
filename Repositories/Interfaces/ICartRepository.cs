using ECommerce_Mvc.Models;
using ECommerce_Mvc.ViewModels;

namespace ECommerce_Mvc.Repositories.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetByBuyerIdAsync(string buyerId);
    Task<CartViewModel?> GetByBuyerIdWithItemsAsync(string buyerId);
    Task AddAsync(Cart cart);
    void Remove(Cart cart);
    Task SaveChangesAsync();
}
