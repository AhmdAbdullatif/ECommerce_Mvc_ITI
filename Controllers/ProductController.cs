using ECommerce_Mvc.Constants;
using ECommerce_Mvc.Models;
using ECommerce_Mvc.Services.Interfaces;
using ECommerce_Mvc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ECommerce_Mvc.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(
        IProductService productService,
        ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        string? searchText,
        int? categoryId,
        string? sortOrder)
    {
        List<Product> products =
            await _productService.GetAllAsync(
                searchText,
                categoryId,
                sortOrder);

        ViewBag.SearchText = searchText;
        ViewBag.CategoryId = categoryId;
        ViewBag.SortOrder = sortOrder;

        ViewBag.Categories =
            new SelectList(
                await _categoryService.GetAllAsync(),
                "Id",
                "Name",
                categoryId);

        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        Product? product =
            await _productService.GetByIdAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    [Authorize(Roles = AuthorizationConstants.SELLERS)]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();

        return View(new ProductCreateViewModel());
    }

    [Authorize(Roles = AuthorizationConstants.SELLERS)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ProductCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(model.CategoryId);

            return View(model);
        }

        string? userId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        try
        {
            await _productService.CreateAsync(
                model.CategoryId,
                model.Name,
                model.Description,
                model.Quantity,
                model.Price,
                model.PictureUri,
                userId);
        }
        catch (ArgumentException)
        {
            return BadRequest(
                "Category does not exist.");
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = AuthorizationConstants.SELLERS)]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        string? userId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        Product? product =
            await _productService.GetSellerProductAsync(
                id,
                userId);

        if (product is null)
        {
            return Forbid();
        }

        ProductEditViewModel model =
            new ProductEditViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId,
                PictureUri = product.PictureUri
            };

        await LoadCategoriesAsync(
            product.CategoryId);

        return View(model);
    }

    [Authorize(Roles = AuthorizationConstants.SELLERS)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        ProductEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(
                model.CategoryId);

            return View(model);
        }

        string? userId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        try
        {
            await _productService.UpdateAsync(
                model.Id,
                userId,
                model.CategoryId,
                model.Name,
                model.Description,
                model.Quantity,
                model.Price,
                model.PictureUri);
        }
        catch (ArgumentException)
        {
            return BadRequest(
                "Category does not exist.");
        }
        catch (InvalidOperationException)
        {
            return Forbid();
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = AuthorizationConstants.SELLERS)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        string? userId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        try
        {
            await _productService.DeleteAsync(
                id,
                userId);
        }
        catch (InvalidOperationException)
        {
            return Forbid();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategoriesAsync(
        int? selectedCategoryId = null)
    {
        ViewBag.Categories =
            new SelectList(
                await _categoryService.GetAllAsync(),
                "Id",
                "Name",
                selectedCategoryId);
    }
}