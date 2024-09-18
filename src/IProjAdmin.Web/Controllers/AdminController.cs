using IProj.DataAccess.Interfaces.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace IProjAdmin.Web.Controllers;

[Authorize]
public class AdminController : Controller
{
    private IUserRepository _userRepository;

    public AdminController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    [HttpGet]
    public IActionResult SendMessage()
    {
        var bossId = HttpContext.Request.Cookies["BossId"];

        if (bossId == null)
        {
            return RedirectToAction("Index", "Home");
        }

        try
        {
            var users = _userRepository.GetAll().Where(u => u.RoleName == "Worker").ToList();
            return View(users);
        }
        catch (Exception ex)
        {
            Log.Error($"Databasega ulanishda xatolik yuz berdi {ex}");
        }
        return View();
    }
    public IActionResult Main()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Logout()
    {
        foreach (var cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie, new CookieOptions
            {
                Domain = "iproj.uz",
                Path = "/",
                Secure = true,
                HttpOnly = true
            });
        }

        return SignOut(new AuthenticationProperties
        {
            RedirectUri = Url.Action("Index", "Home")
        }, "Cookies", "oidc");
    }


    [HttpGet]
    public IActionResult ViewMessageModal()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Reload()
    {
        return RedirectToAction("Main");
    }

}
