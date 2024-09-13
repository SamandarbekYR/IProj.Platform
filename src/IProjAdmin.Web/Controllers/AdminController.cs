using IProj.DataAccess.Interfaces.Users;
using IProj.DataAccess.Repositories.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace IProjAdmin.Web.Controllers
{
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
            //var bossId = HttpContext.Request.Cookies["BossId"];

            //if (bossId == null)
            //{
            //    return RedirectToAction("Login", "Accaunt");
            //}

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
                Response.Cookies.Delete(cookie);
            }

            return SignOut(new AuthenticationProperties
            {
                RedirectUri = Url.Action("Login", "Accaunt")
            }, "Cookies", "oidc");
        }
    }
}
