namespace TravelAgentWeb.Models;

public class WebhookSecret
{
    public int Id { get; set; }
    public string Secret { get; set; } = default!;
    public string Publisher { get; set; } = default!;
}