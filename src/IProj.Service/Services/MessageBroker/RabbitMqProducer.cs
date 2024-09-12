using IProj.DataAccess.Interfaces.MessageBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

namespace IProj.Service.Services.MessageBroker;

public class RabbitMqProducer : IRabbitMqProducer
{
    private readonly IConfigurationSection _config;
    private readonly ILogger<RabbitMqProducer> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqProducer(IConfiguration configuration,
                            ILogger<RabbitMqProducer> logger)
    {
        try
        {
            _config = configuration.GetSection("MessageBroker");
            _logger = logger;

            var factory = new ConnectionFactory()
            {
                HostName = _config["Host"],
                Port = int.Parse(_config["Port"]!),
                UserName = _config["Username"],
                Password = _config["Password"],
                VirtualHost = _config["VirtualHost"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _config["Queue"],
                                  durable: true, exclusive: false,
                                  autoDelete: false, arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        }

        catch (Exception ex)
        {
            _logger.LogError($"RabbitMq ga ulanishda xatolik yuz berdi: {ex}");
        }
    }
    public void SendMessage(string message)
    {
        try
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "",
                                 routingKey: "MessageQueue",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine("Sent {0}", message);
        }

        catch(Exception ex) 
        {
            _logger.LogError($"RabbitMqga ma'lumot yozishda xatolik yuz berdi: {ex}");
        }
    }
    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
