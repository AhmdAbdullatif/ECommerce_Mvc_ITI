using ECommerce_Mvc.Models;
using ECommerce_Mvc.Requests;
using ECommerce_Mvc.ViewModels;

namespace ECommerce_Mvc.Services.Interfaces;

public interface ICartService
{
    Task AddItemToCart(string userEmail, AddItemToCartRequest request);
    Task<CartViewModel> GetCartAsync(string buyerId);
}
