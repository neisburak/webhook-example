namespace AirlineSendAgent.Dtos;

public class MessageForNotification
{
    public string Id { get; set; } = default!;
    public string WebhookType { get; set; } = default!;
    public string FlightCode { get; set; } = default!;
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
}