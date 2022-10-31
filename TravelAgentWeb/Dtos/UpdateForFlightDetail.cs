namespace TravelAgentWeb.Dtos;

public class UpdateForFlightDetail
{
    public string Publisher { get; set; } = default!;
    public string Secret { get; set; } = default!;
    public string FlightCode { get; set; } = default!;
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public string WebhookType { get; set; } = default!;
}