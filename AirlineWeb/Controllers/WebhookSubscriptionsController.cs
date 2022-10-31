using AirlineWeb.Data;
using AirlineWeb.Dtos;
using AirlineWeb.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebhookSubscriptionsController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public WebhookSubscriptionsController(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("{secret}", Name = "GetSubscriptionAsync")]
    public async Task<ActionResult<WebhookSubscriptionForView>> GetAsync(string secret)
    {
        var subscription = await _context.WebhookSubscriptions.FirstOrDefaultAsync(f => f.Secret == secret);
        if (subscription is null) return NotFound();
        return Ok(_mapper.Map<WebhookSubscriptionForView>(subscription));
    }

    [HttpPost]
    public async Task<ActionResult<WebhookSubscriptionForView>> PostAsync([FromBody] WebhookSubscriptionForCreate subscriptionForCreate)
    {
        var subscription = await _context.WebhookSubscriptions.FirstOrDefaultAsync(f => f.WebhookUri == subscriptionForCreate.WebhookUri);
        if (subscription is null)
        {
            subscription = _mapper.Map<WebhookSubscription>(subscriptionForCreate);
            subscription.Secret = Guid.NewGuid().ToString();
            subscription.WebhookPublisher = "PanAus";

            try
            {
                await _context.AddAsync(subscription);
                await _context.SaveChangesAsync();

                var dto = _mapper.Map<WebhookSubscriptionForView>(subscription);
                return CreatedAtRoute("GetSubscriptionAsync", new { Secret = dto.Secret }, dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        return NoContent();
    }
}