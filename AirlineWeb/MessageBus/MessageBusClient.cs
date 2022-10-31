using System.Text;
using System.Text.Json;
using AirlineWeb.Dtos;
using RabbitMQ.Client;

namespace AirlineWeb.MessageBus;

public class MessageBusClient : IMessageBusClient
{
    private readonly ILogger<MessageBusClient> _logger;

    public MessageBusClient(ILogger<MessageBusClient> logger)
    {
        _logger = logger;
    }

    public void SendMessage(MessageForNotification messageForNotification)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

        var message = JsonSerializer.Serialize(messageForNotification);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "trigger", routingKey: string.Empty, basicProperties: null, body: body);
        _logger.LogInformation("Message published on message bus.");
    }
}