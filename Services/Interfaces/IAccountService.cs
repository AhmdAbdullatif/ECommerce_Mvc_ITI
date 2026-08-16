using ECommerce_Mvc.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ECommerce_Mvc.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<(SignInResult Result, string? ErrorMessage)> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}
