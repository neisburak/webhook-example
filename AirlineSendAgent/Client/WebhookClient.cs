using System.Net.Http.Headers;
using System.Text.Json;
using AirlineSendAgent.Dtos;

namespace AirlineSendAgent.Client;

public class WebhookClient : IWebhookClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public WebhookClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task SendWebhookNotificationAsync(ChangePayloadForFlightDetail flightDetail)
    {
        var payload = JsonSerializer.Serialize(flightDetail);

        var httpClient = _httpClientFactory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Post, flightDetail.WebhookUri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        request.Content = new StringContent(payload);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        try
        {
            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unsuccessful: {ex.Message}");
        }
    }
}