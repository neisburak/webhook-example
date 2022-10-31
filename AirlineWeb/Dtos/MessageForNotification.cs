namespace AirlineWeb.Dtos;

public class MessageForNotification
{
    public string Id => Guid.NewGuid().ToString();
    public string WebhookType { get; set; } = default!;
    public string FlightCode { get; set; } = default!;
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
}