using IProj.DataAccess.Interfaces.MessageBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

public class RabbitMqProducer : IRabbitMqProducer, IDisposable
{
    private static IConnection? _connection;
    private static IModel? _channel;
    private readonly ILogger<RabbitMqProducer> _logger;
    private readonly IConfigurationSection _config;

    public RabbitMqProducer(IConfiguration configuration, ILogger<RabbitMqProducer> logger)
    {
        _config = configuration.GetSection("MessageBroker");
        _logger = logger;

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
        catch (Exception ex)
        {
            _logger.LogError($"RabbitMqga ma'lumot yozishda xatolik yuz berdi: {ex}");
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
}