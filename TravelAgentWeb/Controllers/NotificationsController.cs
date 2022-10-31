using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgentWeb.Data;
using TravelAgentWeb.Dtos;

namespace TravelAgentWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly TravelAgentContext _context;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(TravelAgentContext context, ILogger<NotificationsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] UpdateForFlightDetail flightDetail)
    {
        _logger.LogInformation($"Webhook received from: {flightDetail.Publisher}");

        var model = await _context.SubscriptionSecrets.FirstOrDefaultAsync(f => f.Publisher == flightDetail.Publisher && f.Secret == flightDetail.Secret);
        if (model is null)
        {
            _logger.LogWarning("Invalid secret - Ignore webhook.");
            return BadRequest();
        }

        _logger.LogInformation($"Valid webhook! Old price: {flightDetail.OldPrice}, New price: {flightDetail.NewPrice}");
        return Ok();
    }
}