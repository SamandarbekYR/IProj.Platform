using IProj.DataAccess.Interfaces.Users;
using IProj.Web.Helpers;
using IProj.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IProj.Web.Controllers;

[Authorize]
public class AccauntController : Controller
{
    private IUserRepository _service;
    private AppSettings _appSettings;

    public AccauntController(IUserRepository service,
                             IOptions<AppSettings> appSettings)
    {
        _service = service;
        _appSettings = appSettings.Value;
    }

    [HttpGet]
    public async Task<IActionResult> Login()
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
                    Domain = "iproj.uz",
                    Path = "/",
                    Expires = DateTimeOffset.UtcNow.AddDays(7)

                });

            if (userRole.Equals("Worker"))
            {
                if (!string.IsNullOrEmpty(Gmail) && userId != null)
                {
                    HttpContext.Response.Cookies.Append("WorkerId", userId.ToString()!, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Domain = _appSettings.Domain,
                        Path = "/",
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    });
                }

                return RedirectToAction("Worker", "Messages");
            }

            else
            {
                return Redirect(_appSettings.RedirectUrl);
            }
        }
        return View();
    }
}
