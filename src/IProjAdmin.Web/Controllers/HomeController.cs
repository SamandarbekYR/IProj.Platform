using IProjAdmin.Web.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IProjAdmin.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ILogger _logger;

        public HomeController(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            try
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
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    });

                    if (userRole.Equals("Owner"))
                    {
                        if (!string.IsNullOrEmpty(Gmail) && userId != null)
                        {
                            HttpContext.Response.Cookies.Append("BossId", userId.ToString()!, new CookieOptions
                            {
                                HttpOnly = true,
                                Expires = DateTimeOffset.UtcNow.AddDays(7)
                            });
                        }

                        return RedirectToAction("Main", "Admin");
                    }

                    else
                    {
                        return Redirect("https://iproj.uz");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Home controllerdagi indexda xatolik yuz berdi: {ex}");
                return View();
            }
        }
    }
}
