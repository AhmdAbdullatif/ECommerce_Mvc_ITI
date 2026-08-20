
using ECommerce_Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Mvc.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerController : Controller
    {
        private readonly ISellerService _sellerService;

        public SellerController(
            ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
