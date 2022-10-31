namespace AirlineSendAgent.Dtos;

public class ChangePayloadForFlightDetail
{
    public string WebhookUri { get; set; } = default!;
    public string WebhookType { get; set; } = default!;
    public string Publisher { get; set; } = default!;
    public string Secret { get; set; } = default!;
    public string FlightCode { get; set; } = default!;
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
}