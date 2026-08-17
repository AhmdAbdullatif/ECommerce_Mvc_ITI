using ECommerce_Mvc.Models;
using ECommerce_Mvc.Repositories.Interfaces;
using ECommerce_Mvc.Requests;
using ECommerce_Mvc.Services.Interfaces;
using ECommerce_Mvc.ViewModels;

namespace ECommerce_Mvc.Services.Implementations;

public class CartService(IProductRepository productRepository,
    ICartRepository cartRepository) : ICartService
{
    public async Task AddItemToCart(string userEmail, AddItemToCartRequest request)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new ArgumentException($"Product with ID: {request.ProductId} not found.");

        var cart = await cartRepository.GetByBuyerIdAsync(userEmail);
        if (cart == null)
        {
            cart = new Cart(userEmail);
            await cartRepository.AddAsync(cart);
        }

        cart.AddItem(product.Id, request.Quantity, product.Price);
        await cartRepository.SaveChangesAsync();
    }

    public async Task<CartViewModel> GetCartAsync(string buyerId)
    {
        var cartViewModel = await cartRepository.GetByBuyerIdWithItemsAsync(buyerId);
        Cart cart = null!;
        if (cartViewModel == null)
        {
            cart = new Cart(buyerId);
            await cartRepository.AddAsync(cart);
            await cartRepository.SaveChangesAsync();
        }

        return cartViewModel ?? new CartViewModel() { CartId = cart.Id };
    }
}
