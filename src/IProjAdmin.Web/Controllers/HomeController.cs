using IProjAdmin.Web.Helpers;
using IProjAdmin.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IProjAdmin.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly AppSettings _appSettings;

    public HomeController(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }
    public async Task<IActionResult> Index()
    {

        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var claimsPrincipal = accessToken!.DecodeJwtToken();
        var userRole = claimsPrincipal.GetRole();
        (string? Gmail, Guid? userId) = claimsPrincipal.GetEmailAndId();

        if (userRole != null && !string.IsNullOrEmpty(Gmail))
        {

            HttpContext.Response.Cookies.Append("UserGmail", Gmail, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Domain = _appSettings.Domain,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });


            if (userRole.Equals("Owner"))
            {
                if (!string.IsNullOrEmpty(Gmail) && userId != null)
                {
                    HttpContext.Response.Cookies.Append("BossId", userId.ToString()!, new CookieOptions
                    {

                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Domain = _appSettings.Domain,
                        Path = "/",
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    });
                }

                return RedirectToAction("Main", "Admin");
            }

            else
            {
                return Redirect(_appSettings.RedirectUrl);
            }
        }

        return View();
    }
}