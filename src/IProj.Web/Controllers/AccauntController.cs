using IProj.DataAccess.Interfaces.Users;
using IProj.Web.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IProj.Web.Controllers;

[Authorize]
public class AccauntController : Controller
{
    private IUserRepository _service;

    public AccauntController(IUserRepository service)
    {
        _service = service;
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
                        Domain = "iproj.uz",
                        Path = "/",
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    });
                }

                return RedirectToAction("Worker", "Messages");
            }

            else
            {
                return Redirect("https://admin.iproj.uz");
            }
        }
        return View();
    }
}
