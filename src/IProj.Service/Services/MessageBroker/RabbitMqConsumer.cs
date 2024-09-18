using IProj.DataAccess.Interfaces.Messages;
using IProj.Domain.Entities.Messages;
using IProj.Service.DTOs.Messages;
using IProj.Service.Interfaces.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace IProj.Service.Services.MessageBroker;

public class RabbitMqConsumer : BackgroundService
{
    private readonly ILogger<RabbitMqConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfigurationSection _config;
    private readonly IModel _channel;
    private readonly IConnection _connection;
    
    public RabbitMqConsumer(IConfiguration config,
                            IServiceProvider serviceProvider,
                            ILogger<RabbitMqConsumer> logger)

    {

        _logger = logger;
        _serviceProvider = serviceProvider;
        _config = config.GetSection("MessageBroker");

        try
        {
            if (_connection == null)
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _config["Host"],
                    Port = int.Parse(_config["Port"]!),
                    UserName = _config["Username"],
                    Password = _config["Password"],
                    VirtualHost = _config["VirtualHost"]
                };

                _connection = factory.CreateConnection();
            }

            if (_channel == null)
            {
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: "MessageQueue",
                                      durable: true, exclusive: false,
                                      autoDelete: false, arguments: null);
                _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            }
        }
        catch(Exception ex)
        {
            _logger.LogError($"RabbitMqga consumer ulanishda xatolik yuz berdi: {ex}");
        }
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        StartListening();
        return Task.CompletedTask;
    }
    private void StartListening()
    {
        try
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
                    var emailSenderService = scope.ServiceProvider.GetRequiredService<ISendMessageToEmailService>();

                    var body = ea.Body.ToArray();
                    var messageContent = Encoding.UTF8.GetString(body);

                    var messageGet = JsonConvert.DeserializeObject<SendMessageToMessageBroker>
                                                                                        (messageContent)!;

                    if (!string.IsNullOrEmpty(messageGet.MessageContent))
                    {
                        Message message = new Message
                        {
                            Id = messageGet.MessageId,
                            SenderId = messageGet.SenderId,
                            ReceiverId = messageGet.ReceiverId,
                            MessageContent = messageGet.MessageContent,
                            IsRead = messageGet.IsRead,
                            SendTime = messageGet.SendTime,
                        };

                        await _messageRepository.Add(message);

                        SendMessageToEmailDto messageDto = new()
                        {
                            Title = "Boss's message\n",
                            Body = message.MessageContent,
                            To = messageGet.ReceiverGmail
                        };
                        await emailSenderService.SendEmailAsync(messageDto);
                    }
                }
                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            };
            _channel.BasicConsume(queue: "MessageQueue",
                                      autoAck: false,
                                      consumer: consumer);
        }
        catch(Exception ex)
        {
            _logger.LogError($"RabbitMq dan malumot olishda xatolik yuz berdi: {ex}");
        }
    }

    public override void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
        base.Dispose();
    }
}
