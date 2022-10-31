namespace AirlineWeb.Dtos;

public class WebhookSubscriptionForCreate
{
    public string WebhookUri { get; set; } = default!;
    public string WebhookType { get; set; } = default!;
}