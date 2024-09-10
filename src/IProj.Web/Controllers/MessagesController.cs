using IProj.DataAccess.Interfaces.MessageBroker;
using IProj.DataAccess.Interfaces.Messages;
using IProj.DataAccess.Interfaces.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Boss()
        {
            var bossId = HttpContext.Request.Cookies["BossId"];

            if (bossId == null)
            {
                return RedirectToAction("Login", "Accaunt");
            }

            var users = _usersrepository.GetAll().Where(u => u.RoleName == "Worker").ToList();
            return View(users);
        }
        [HttpPost]
        public IActionResult SendMessage()
        {
            return RedirectToAction("Boss");
        }

        [HttpGet]
        public IActionResult Worker()
        {
            var workerId = HttpContext.Request.Cookies["WorkerId"];

            if (workerId == null)
            {
                return RedirectToAction("Login", "Accaunt");
            }


            var messages = _message.GetAll().Where(i => i.ReceiverId == Guid.Parse(workerId) &&
                                                   i.IsRead == false).ToList();

            var newMessageCount = messages.Count;
            ViewData["newMessageCount"] = newMessageCount;
            return View(messages);
        }

        [HttpGet]
        public IActionResult GetMessagesForWorker()
        {
            var workerId = HttpContext.Request.Cookies["WorkerId"];

            if (workerId == null)
            {
                return RedirectToAction("Login", "Accaunt");
            }

            var messages = _message.GetAll().Where(items => items.ReceiverId == Guid.Parse(workerId)).ToList();

            return Json(messages);
        }


        //[HttpGet]
        //public async Task<IActionResult> GetMessages()
        //{
        //    if (!HttpContext.Request.Cookies.ContainsKey("BossId"))
        //    {
        //        return RedirectToAction("Login", "Accaunt");
        //    }

        //    else
        //    {
        //        var BossId = HttpContext.Request.Cookies["BossId"];
        //        var messages = await _message.GetBySenderIdAllMessagesAsync(Guid.Parse(BossId!));
        //        List<MessageView> messageViews = new List<MessageView>();

        //        foreach (var message in messages)
        //        {
        //            var receiverInfo = await _usersrepository.GetById(message.ReceiverId);

        //            if (receiverInfo is not null)
        //            {
        //                MessageView messageView = new MessageView();
        //                messageView.ReceiveName = receiverInfo.FirstName;
        //                messageView.MessageContent = message.MessageContent;
        //                messageView.SendTime = message.SendTime;
        //                messageView.ReadTime = message.ReadTime;
        //                messageView.ReadStatus = message.IsRead;
        //                messageViews.Add(messageView);
        //            }
        //        }

        //        return Json(messageViews);
        //    }
        //}

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
