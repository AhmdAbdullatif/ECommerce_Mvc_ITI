using ECommerce_Mvc.Models;
using ECommerce_Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace ECommerce_Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    // حقن السيرفيسز الخاصة بالمنتجات والأقسام
    public HomeController(
        IProductService productService,
        ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    // تحويل الـ Action لـ Async لاستقبال البيانات
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


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
