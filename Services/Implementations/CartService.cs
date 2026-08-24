using ECommerce_Mvc.Models;
using ECommerce_Mvc.Models.Requests;
using ECommerce_Mvc.Repositories.Interfaces;
using ECommerce_Mvc.Services.Interfaces;
using ECommerce_Mvc.ViewModels;

namespace ECommerce_Mvc.Services.Implementations;

public class CartService(IProductRepository productRepository,
    ICartRepository cartRepository) : ICartService
{
    public async Task AddItemAsync(string userEmail, AddItemToCartRequest request)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new ArgumentException($"Product with ID: {request.ProductId} not found.");
        
        if (product.Quantity < request.Quantity)
            throw new InvalidOperationException($"Product with ID: {product.Id} has less items than {request.Quantity}.");

        var cart = await cartRepository.GetByBuyerIdAsync(userEmail);
        if (cart == null)
        {
            cart = new Cart(userEmail);
            await cartRepository.AddAsync(cart);
        }

        cart.AddItem(product.Id, request.Quantity, product.Price);
        await cartRepository.SaveChangesAsync();
    }

    public async Task DeleteItemAsync(string buyerId, int productId)
    {
        var cart = await cartRepository.GetByBuyerIdAsync(buyerId);
        if (cart == null)
        {
            cart = new Cart(buyerId);
            await cartRepository.AddAsync(cart);
            await cartRepository.SaveChangesAsync();
            return;
        }

        cart.RemoveItem(productId);
        await cartRepository.SaveChangesAsync();
    }

    public async Task UpdateItemQuantityAsync(string buyerId, UpdateCartItemQuantityRequest request)
    {
        var cart = await cartRepository.GetByBuyerIdAsync(buyerId)
                   ?? throw new ArgumentException("Cart not found.");
        
        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product == null) 
            throw new ArgumentException($"Product with ID: {request.ProductId} is not found."); 
        
        if (product.Quantity < request.Quantity)
            throw new InvalidOperationException($"Product with ID: {product.Id} has less items than {request.Quantity}.");

        cart.UpdateItemQuantity(request.ProductId, request.Quantity);
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
    
    public async Task TransferCartAsync(string anonymousId, string userEmail)
    {
        var anonymousCart = await cartRepository.GetByBuyerIdAsync(anonymousId);
        if (anonymousCart == null)
            return;

        var userCart = await cartRepository.GetByBuyerIdAsync(userEmail);
        if (userCart == null)
        {
            userCart = new Cart(userEmail);
            await cartRepository.AddAsync(userCart);
            await cartRepository.SaveChangesAsync();
        }

        foreach (var item in anonymousCart.Items)
        {
            userCart.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
        }

        cartRepository.Remove(anonymousCart);
        await cartRepository.SaveChangesAsync();
    }
}