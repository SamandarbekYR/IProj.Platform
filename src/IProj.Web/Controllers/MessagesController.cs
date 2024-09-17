using IProj.DataAccess.Interfaces.MessageBroker;
using IProj.DataAccess.Interfaces.Messages;
using IProj.DataAccess.Interfaces.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MVCLearn.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private IUserRepository _usersrepository;
        private readonly IRabbitMqProducer _rabbitMQProducer;
        private readonly IMessageRepository _message;

        public MessagesController(IUserRepository userRepository,
                                  IRabbitMqProducer rabbitMQProducer,
                                  IMessageRepository message)
        {
            _usersrepository = userRepository;
            _rabbitMQProducer = rabbitMQProducer;
            _message = message;
        }

        [HttpGet]
        public IActionResult Worker()
        {
            var userGmail = HttpContext.Request.Cookies["UserGmail"];
            if (userGmail == null)
            {
                return RedirectToAction("Login", "Accaunt");
            }

            var receiverId = _usersrepository.GetAll().FirstOrDefault(u => u.Gmail == userGmail);

            if (receiverId != null)
            {
                var messages = _message.GetAll().Where(i => i.ReceiverId == receiverId.Id &&
                                                       i.IsRead == false).ToList();

                var newMessageCount = messages.Count;
                ViewData["newMessageCount"] = newMessageCount;
                return View(messages);
            }

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
                RedirectUri = Url.Action("Login", "Accaunt")
            }, "Cookies", "oidc");
        }

    }
}
