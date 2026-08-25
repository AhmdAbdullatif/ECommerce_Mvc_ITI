using System.ClientModel.Primitives;
using ECommerce_Mvc.Configuration;
using ECommerce_Mvc.Data;
using ECommerce_Mvc.Models;
using ECommerce_Mvc.Services.Interfaces;
using ECommerce_Mvc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

namespace ECommerce_Mvc.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAnonymousCartManager _anonymousCartManager;
        private readonly StripeOptions _stripeOptions;

        public OrdersController(AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IAnonymousCartManager anonymousCartManager,
            IOptions<StripeOptions> stripeOptions)
        {
            _context = context;
            _userManager = userManager;
            _anonymousCartManager = anonymousCartManager;
            _stripeOptions = stripeOptions.Value;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View(new CheckoutViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "يرجى ملء جميع الحقول المطلوبة بشكل صحيح.";
                return View("Checkout", model);
            }

            var userId = _userManager.GetUserId(User);
            var userEmail = _anonymousCartManager.GetOrSetCookieAndUserEmail(HttpContext);

            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.BuyerId == userEmail || c.BuyerId == userId);

            if (cart == null || !cart.Items.Any())
            {
                TempData["ErrorMessage"] = "سلة المشتريات فارغة في قاعدة البيانات! تأكد من إضافة منتجات أولاً.";
                return View("Checkout", model);
            }

            var domain = _stripeOptions.Domain;
            var lineItems = new List<SessionLineItemOptions>();

            foreach (var item in cart.Items)
            {
                var unitAmount = item.UnitPrice > 0 ? (long)(item.UnitPrice * 100) : 1000;

                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = unitAmount,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product?.Name ?? "منتج",
                        },
                    },
                    Quantity = item.Quantity > 0 ? item.Quantity : 1,
                });
            }

            // تم إضافة حقول الدولة والمحافظة لتخزينها في Metadata وتجنب خطأ قاعدة البيانات
            var sessionOptions = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = domain + "/Orders/OrderSuccess?sessionId={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "/Orders/Checkout",
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", userId },
                    { "UserEmail", userEmail },
                    { "Country", model.Country ?? "Egypt" },
                    { "State", model.State ?? "Beheira" },
                    { "City", model.City ?? "Unknown" },
                    { "Street", model.Street ?? "Unknown" }
                }
            };

            var requestOptions = new Stripe.RequestOptions()
            {
                ApiKey = _stripeOptions.SecretKey,
            };

            var service = new SessionService();
            Session session;

            try
            {
                session = await service.CreateAsync(sessionOptions, requestOptions);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء الاتصال ببوابة الدفع: " + ex.Message;
                return View("Checkout", model);
            }

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [HttpGet]
        public async Task<IActionResult> OrderSuccess(string sessionId)
        {
            var service = new SessionService();
            var session = service.Get(sessionId);

            if (session.PaymentStatus == "paid")
            {
                var userId = session.Metadata["UserId"];
                var userEmail = session.Metadata.ContainsKey("UserEmail") ? session.Metadata["UserEmail"] : userId;

                // استرجاع كافة بيانات العنوان المرسلة
                var country = session.Metadata.ContainsKey("Country") ? session.Metadata["Country"] : "Egypt";
                var state = session.Metadata.ContainsKey("State") ? session.Metadata["State"] : "Beheira";
                var city = session.Metadata["City"];
                var street = session.Metadata["Street"];

                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.BuyerId == userEmail || c.BuyerId == userId);

                if (cart != null && cart.Items.Any())
                {
                    var orderItems = cart.Items.Select(ci => new OrderItem(ci.ProductId, ci.Quantity, ci.UnitPrice)).ToList();

                    // استخدام الـ Constructor الذي يقبل الحقول الأربعة لمنع حدوث خطأ NULL في SQL
                    var shippingAddress = new Address(country, state, city, street);
                    var order = new Order(userId, shippingAddress, orderItems);

                    _context.Orders.Add(order);
                    _context.Carts.Remove(cart);
                    await _context.SaveChangesAsync();
                }

                return View();
            }

            return RedirectToAction("Checkout");
        }
    }
}