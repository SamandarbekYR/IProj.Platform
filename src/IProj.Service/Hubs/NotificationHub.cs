using IProj.DataAccess.Interfaces.MessageBroker;
using IProj.DataAccess.Interfaces.Messages;
using IProj.DataAccess.Interfaces.Users;
using IProj.Service.DTOs.Messages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace IProj.Service.Hubs;

public class NotificationHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> _connection = new ConcurrentDictionary<string, string>();
    private IUserRepository _userRepository;
    private IMessageRepository _message;
    private IRabbitMqProducer _producerService;
    private ILogger<NotificationHub> _logger;

    public NotificationHub(IUserRepository userRepository,
                           IMessageRepository message,
                           IRabbitMqProducer producerService, 
                           ILogger<NotificationHub> logger)
    {
        _userRepository = userRepository;
        _message = message;
        _producerService = producerService;
        _logger = logger;
    }
    public override async Task OnConnectedAsync()
    {
        try
        {
            var httpContext = Context.GetHttpContext();
            var userGmail = httpContext!.Request.Cookies["UserGmail"];

            if (!string.IsNullOrEmpty(userGmail))
            {
                _connection.TryAdd(Context.ConnectionId, userGmail);
                var user = await _userRepository.UpdateUserIsOnline(userGmail, true);

                if (user is not null)
                {
                    await Clients.All.SendAsync("UpdateUserStatus", user.Gmail, user.IsOnline);
                }
            }
            await base.OnConnectedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Hubga ulanishda xatolik yuz berdi {ex}");
        }
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            if (_connection.TryRemove(Context.ConnectionId, out var userGmail))
            {
                var user = await _userRepository.UpdateUserIsOnline(userGmail, false);

                if (user != null)
                {
                    await Clients.All.SendAsync("UpdateUserStatus", user.Gmail, user.IsOnline);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Hubdan chiqishda xatolik yuz berdi: {ex}");
        }
    }
    public async Task SendMessageToSelectedUsers(SendMessageToUserDto userInfo, string message)
    {
        try
        {
            var connectionId = _connection.FirstOrDefault(x => x.Value == userInfo.Email).Key;
            var messageId = SaveMessageToMessageBroker(message, userInfo);

            if (messageId is not null)
            {
                SendMessageToSelectedUsers sendMessageDto = new SendMessageToSelectedUsers
                {
                    MessageId = messageId,
                    MessageContent = message,
                };

                if (connectionId != null)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", sendMessageDto);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"SignalRda habar jo'natishda xatolik yuz berdi: {ex}");
        }
    }
    public async Task UpdateMessageStatus(string[] messageIds, bool isRead)
    {
        try
        {
            foreach (var messageId in messageIds)
            {
                var message = await _message.UpdateMessageStatus(Guid.Parse(messageId), isRead);
                if (message != null)
                {
                    var user = await _userRepository.GetAll().FirstOrDefaultAsync(item => item.Id == message.ReceiverId);
                    if (user != null)
                    {
                        MessageViewDto messageView = new MessageViewDto
                        {
                            ReceiveName = user.FirstName,
                            MessageContent = message.MessageContent,
                            SendTime = message.SendTime,
                            ReadTime = message.ReadTime,
                            ReadStatus = message.IsRead
                        };
                        await Clients.All.SendAsync("MessageStatusUpdated", messageView);
                    }
                    else
                    {
                        Console.WriteLine($"Error: User with ReceiverId {message.ReceiverId} not found.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: Message with ID {messageId} not found or failed to update.");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"UpdateMessageStatusIsRead methodda xatolik yuz berdi: {ex}");
        }
    }

    public Guid? SaveMessageToMessageBroker(string message, SendMessageToUserDto userInfo)
    {
        try
        {
            var httpContext = Context.GetHttpContext();
            var SenderId = httpContext!.Request.Cookies["BossId"];

            SendMessageToMessageBroker messageInfo = new SendMessageToMessageBroker();
            messageInfo.MessageId = Guid.NewGuid();
            messageInfo.MessageContent = message;
            messageInfo.ReceiverId = userInfo.Id;
            messageInfo.ReceiverGmail = userInfo.Email;
            messageInfo.SenderId = Guid.Parse(SenderId!);
            messageInfo.SendTime = DateTime.UtcNow.AddHours(5);
            messageInfo.IsRead = false;

            var json = JsonConvert.SerializeObject(messageInfo);
            _producerService.SendMessage(json);

            return messageInfo.MessageId;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Messageni RabbitMq ga yozishda xatolik yuz berdi: {ex}");
            return null;
        }
    }
}
