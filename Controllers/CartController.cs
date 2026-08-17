using ECommerce_Mvc.Requests;
using ECommerce_Mvc.Services.Implementations;
using ECommerce_Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Mvc.Controllers;

public class CartController(ICartService cartService, IAnonymousCartManager anonymousCartManager) : Controller
{
    [HttpPost]
    public async Task<IActionResult> AddItem([FromForm] AddItemToCartRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userEmail = anonymousCartManager.GetOrSetCookieAndUserEmail(HttpContext);
        try
        {
            await cartService.AddItemToCart(userEmail, request);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
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
}
