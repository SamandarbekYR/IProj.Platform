using IProj.DataAccess.Interfaces.MessageBroker;
using IProj.DataAccess.Interfaces.Messages;
using IProj.DataAccess.Interfaces.Users;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace IProj.Service.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _connection = new ConcurrentDictionary<string, string>();
        private IUserRepository _userRepository;
        private IMessageRepository _messageRepository;
        private IRabbitMqProducer _producerService;
        public NotificationHub(IUserRepository userRepository,
                               IMessageRepository message,
                               IRabbitMqProducer rabbitMqProducer)
        {
            _userRepository = userRepository;
            _messageRepository = message;
            _producerService = rabbitMqProducer;
        }
    }
}
