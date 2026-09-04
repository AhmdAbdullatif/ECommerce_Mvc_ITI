using ECommerce_Mvc.Constants;
using ECommerce_Mvc.Models;
using ECommerce_Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Mvc.Controllers
{

    namespace ECommerce_Mvc.Controllers
    {
        [Authorize] // تأكد من أن أي شخص يدخل هنا مسجل دخوله على الأقل
    public class SellerController : Controller
    {
        private readonly ISellerService _sellerService;
            private readonly UserManager<ApplicationUser> _userManager;

        public SellerController(
                ISellerService sellerService,
                UserManager<ApplicationUser> userManager)
        {
            _sellerService = sellerService;
                _userManager = userManager;
        }

            // ==========================================
            // 1. صلاحيات البائع (Seller)
            // ==========================================
            [Authorize(Roles = "Seller")]
            [HttpGet]
        public IActionResult Index()
        {
                // لوحة تحكم البائع: تعرض إحصائياته أو منتجاته
            return View();
        }


            // ==========================================
            // 2. صلاحيات المستخدم العادي (Customer)
            // ==========================================
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> SubmitRequest()
            {
                var userId = _userManager.GetUserId(User);
                var success = await _sellerService.RequestToBecomeSellerAsync(userId);

                if (success)
                {
                    TempData["SuccessMessage"] = "تم إرسال طلبك للإدارة، في انتظار الموافقة.";
                }
                else
                {
                    TempData["ErrorMessage"] = "حدث خطأ، قد يكون لديك طلب قيد المراجعة بالفعل أو أنك بائع حالياً.";
                }

                // إعادته للصفحة الرئيسية أو لصفحة حسابه الشخصي
                return RedirectToAction("Index", "Home");
            }


            // ==========================================
            // 3. صلاحيات الإدارة (Admin)
            // ==========================================
            [Authorize(Roles = AuthorizationConstants.ADMINISTRATORS)]
            [HttpGet]
            public async Task<IActionResult> PendingRequests()
            {
                // صفحة للإدمن تعرض كل الطلبات المعلقة
                var requests = await _sellerService.GetPendingRequestsAsync();
                return View(requests);
            }

            [Authorize(Roles = AuthorizationConstants.ADMINISTRATORS)]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> ApproveRequest(int id)
            {
                var adminId = _userManager.GetUserId(User);
                var success = await _sellerService.ApproveSellerAsync(id, adminId);

                if (success)
                    TempData["SuccessMessage"] = "تمت الموافقة وتم ترقية المستخدم إلى بائع بنجاح.";
                else
                    TempData["ErrorMessage"] = "حدث خطأ أثناء الموافقة.";

                return RedirectToAction(nameof(PendingRequests));
            }

            [Authorize(Roles = AuthorizationConstants.ADMINISTRATORS)]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> RejectRequest(int id)
            {
                var adminId = _userManager.GetUserId(User);
                var success = await _sellerService.RejectSellerAsync(id, adminId);

                if (success)
                    TempData["SuccessMessage"] = "تم رفض الطلب.";
                else
                    TempData["ErrorMessage"] = "حدث خطأ أثناء الرفض.";

                return RedirectToAction(nameof(PendingRequests));
            }
        }
    }
}
