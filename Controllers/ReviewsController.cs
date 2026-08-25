using ECommerce_Mvc.Data;
using ECommerce_Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Mvc.Controllers;

[Authorize]
public class ReviewsController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReviewsController(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Reviews/Create?productId=5
    [HttpGet]
    public async Task<IActionResult> Create(int productId)
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Unauthorized();

        var productExists = await _context.Products
            .AnyAsync(p => p.Id == productId);

        if (!productExists)
            return NotFound();

        // Customer must have purchased the product
        var hasPurchased = await _context.OrderItems
            .AnyAsync(oi =>
                oi.ProductId == productId &&
                oi.Order != null &&
                oi.Order.UserId == userId);

        if (!hasPurchased)
        {
            TempData["ReviewError"] =
                "You can only review products you have purchased.";

            return RedirectToAction(
                "Details",
                "Product",
                new { id = productId });
        }

        // Customer can review a product only once
        var alreadyReviewed = await _context.Reviews
            .AnyAsync(r =>
                r.ProductId == productId &&
                r.UserId == userId);

        if (alreadyReviewed)
        {
            TempData["ReviewError"] =
                "You have already reviewed this product.";

            return RedirectToAction(
                "Details",
                "Product",
                new { id = productId });
        }

        ViewBag.ProductId = productId;

        return View();
    }

    // POST: Reviews/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        int productId,
        int rating,
        string comment)
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Unauthorized();

        var productExists = await _context.Products
            .AnyAsync(p => p.Id == productId);

        if (!productExists)
            return NotFound();

        // Customer must have purchased the product
        var hasPurchased = await _context.OrderItems
            .AnyAsync(oi =>
                oi.ProductId == productId &&
                oi.Order != null &&
                oi.Order.UserId == userId);

        if (!hasPurchased)
        {
            TempData["ReviewError"] =
                "You can only review products you have purchased.";

            return RedirectToAction(
                "Details",
                "Product",
                new { id = productId });
        }

        // Prevent duplicate reviews
        var alreadyReviewed = await _context.Reviews
            .AnyAsync(r =>
                r.ProductId == productId &&
                r.UserId == userId);

        if (alreadyReviewed)
        {
            TempData["ReviewError"] =
                "You have already reviewed this product.";

            return RedirectToAction(
                "Details",
                "Product",
                new { id = productId });
        }

        // Validate rating
        if (rating < 1 || rating > 5)
        {
            ModelState.AddModelError(
                "rating",
                "Rating must be between 1 and 5.");
        }

        // Validate comment
        if (string.IsNullOrWhiteSpace(comment))
        {
            ModelState.AddModelError(
                "comment",
                "Comment is required.");
        }

        if (comment?.Length > 1000)
        {
            ModelState.AddModelError(
                "comment",
                "Comment cannot exceed 1000 characters.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.ProductId = productId;
            return View();
        }

        var review = new Review(
            productId,
            userId,
            rating,
            comment.Trim());

        _context.Reviews.Add(review);

        await _context.SaveChangesAsync();

        TempData["ReviewSuccess"] =
            "Your review has been added successfully.";

        return RedirectToAction(
            "Details",
            "Product",
            new { id = productId });
    }
}