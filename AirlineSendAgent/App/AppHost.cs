namespace AirlineSendAgent.App;

using System.Text;
using System.Text.Json;
using AirlineSendAgent.Client;
using AirlineSendAgent.Data;
using AirlineSendAgent.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class AppHost : IAppHost
{
    private readonly DataContext _context;
    private readonly IWebhookClient _webhookClient;

    public AppHost(DataContext context, IWebhookClient webhookClient)
    {
        _context = context;
        _webhookClient = webhookClient;
    }

    public void Run()
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

        var queueName = channel.QueueDeclare().QueueName;

        channel.QueueBind(queue: queueName, exchange: "trigger", routingKey: string.Empty);

        var consumer = new EventingBasicConsumer(channel);
        Console.WriteLine("Listening on the message bus...");
        consumer.Received += async (sender, args) =>
        {
            Console.WriteLine("Event is triggered!");
            var payload = Encoding.UTF8.GetString(args.Body.ToArray());
            var message = JsonSerializer.Deserialize<MessageForNotification>(payload);

            if (message is null) return;

            var webhookToSend = new ChangePayloadForFlightDetail
            {
                WebhookType = message.WebhookType,
                OldPrice = message.OldPrice,
                NewPrice = message.NewPrice,
                FlightCode = message.FlightCode,
            };

            foreach (var whs in _context.WebhookSubscriptions.Where(w => w.WebhookType == webhookToSend.WebhookType))
            {
                webhookToSend.WebhookUri = whs.WebhookUri;
                webhookToSend.Secret = whs.Secret;
                webhookToSend.Publisher = whs.WebhookPublisher;

                await _webhookClient.SendWebhookNotificationAsync(webhookToSend);
            }
        };
        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        Console.ReadLine();
    }
}