namespace ECommerce_Mvc.Services.Interfaces;

public interface IAnonymousCartManager
{
    string GetOrSetCookieAndUserEmail(HttpContext httpContext);
    Task TransferAnonymousCartToUserAsync(HttpContext httpContext, string userEmail);
}
