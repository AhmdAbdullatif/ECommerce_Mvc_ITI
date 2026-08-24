using ECommerce_Mvc.Models.Requests;
using ECommerce_Mvc.ViewModels;

namespace ECommerce_Mvc.Services.Interfaces;

public interface ICartService
{
    Task AddItemAsync(string userEmail, AddItemToCartRequest request);
    Task UpdateItemQuantityAsync(string buyerId, UpdateCartItemQuantityRequest request);
    Task<CartViewModel> GetCartAsync(string buyerId);
    Task DeleteItemAsync(string buyerId, int productId);
    Task TransferCartAsync(string anonymousId, string userEmail);
}
