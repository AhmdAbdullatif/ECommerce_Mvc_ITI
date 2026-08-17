using System.Security.Claims;
using ECommerce_Mvc.Constants;
using ECommerce_Mvc.Services.Interfaces;

namespace ECommerce_Mvc.Services.Implementations;

public class AnonymousCartManager : IAnonymousCartManager
{
    public string GetOrSetCookieAndUserEmail(HttpContext httpContext)
    {
        if (httpContext.User.Identity is null)
        {
            throw new ArgumentNullException(nameof(httpContext.User.Identity));
        }

        if (httpContext.User.Identity.IsAuthenticated)
        {
            return httpContext.User.FindFirst(ClaimTypes.Email)!.Value.ToString();
        }
        string? anonymousId = null;

        if (httpContext.Request.Cookies.ContainsKey(CartConstants.CART_COOKIENAME))
        {
            anonymousId = httpContext.Request.Cookies[CartConstants.CART_COOKIENAME];
        }

        if (anonymousId != null) return anonymousId;

        anonymousId = Guid.NewGuid().ToString();

        httpContext.Response.Cookies.Append(CartConstants.CART_COOKIENAME, anonymousId, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,          // false while testing on localhost (http)
            SameSite = SameSiteMode.Lax,  // or Strict
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        });

        return anonymousId;

    }
}
