using ECommerce_Mvc.Constants;
using ECommerce_Mvc.Models;
using ECommerce_Mvc.Services.Interfaces;
using ECommerce_Mvc.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ECommerce_Mvc.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email, // استخدام الإيميل كـ UserName
                PhoneNumber = model.PhoneNumber,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // إضافة المستخدم لصلاحية Customer وتسجيل دخوله
                await _userManager.AddToRoleAsync(user, AuthorizationConstants.SELLERS);
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }

        public async Task<(SignInResult Result, string? ErrorMessage)> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            // التحقق من وجود المستخدم وكونه نشط (Active)
            if (user == null || !user.IsActive)
            {
                return (SignInResult.Failed, "Invalid email or password.");
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            return (result, null);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
