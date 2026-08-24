using ECommerce_Mvc.Models.Requests;
using ECommerce_Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Mvc.Controllers;

public class CartController(ICartService cartService, IAnonymousCartManager anonymousCartManager) : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem([FromForm] AddItemToCartRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userEmail = anonymousCartManager.GetOrSetCookieAndUserEmail(HttpContext);
        try
        {
            await cartService.AddItemAsync(userEmail, request);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException)
        {
            TempData["AddError"] = "Not enough stock";
        }

        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userEmail = anonymousCartManager.GetOrSetCookieAndUserEmail(HttpContext);
        
        var cartViewModel = await cartService.GetCartAsync(userEmail);

        return View(nameof(Index), cartViewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateItemQuantity([FromForm] UpdateCartItemQuantityRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userEmail = anonymousCartManager.GetOrSetCookieAndUserEmail(HttpContext);
        try
        {
            await cartService.UpdateItemQuantityAsync(userEmail, request);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException)
        {
            TempData["UpdateError"] = "Not enough stock";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteItem(int productId)
    {
        var userEmail = anonymousCartManager.GetOrSetCookieAndUserEmail(HttpContext);

        await cartService.DeleteItemAsync(userEmail, productId);

        return RedirectToAction(nameof(Index));
    }

}