using AirlineWeb.Data;
using AirlineWeb.Dtos;
using AirlineWeb.MessageBus;
using AirlineWeb.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IMessageBusClient _messageBus;
    private readonly ILogger<FlightsController> _logger;

    public FlightsController(DataContext context, IMapper mapper, ILogger<FlightsController> logger, IMessageBusClient messageBus)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _messageBus = messageBus;
    }

    [HttpGet("{id}", Name = "GetFlightAsync")]
    public async Task<ActionResult<FlightDetailForView>> GetAsync(int id)
    {
        var subscription = await _context.FlightDetails.FirstOrDefaultAsync(f => f.Id == id);
        if (subscription is null) return NotFound();
        return Ok(_mapper.Map<FlightDetailForView>(subscription));
    }

    [HttpPost]
    public async Task<ActionResult<FlightDetailForView>> PostAsync([FromBody] FlightDetailForUpsert flightDetailForCreate)
    {
        var detail = await _context.FlightDetails.FirstOrDefaultAsync(f => f.Code == flightDetailForCreate.Code);
        if (detail is null)
        {
            detail = _mapper.Map<FlightDetail>(flightDetailForCreate);

            try
            {
                await _context.AddAsync(detail);
                await _context.SaveChangesAsync();

                var dto = _mapper.Map<FlightDetailForView>(detail);
                return CreatedAtRoute("GetFlightAsync", new { Code = dto.Code }, dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] FlightDetailForUpsert flightDetailForUpdate)
    {
        var detail = await _context.FlightDetails.FirstOrDefaultAsync(f => f.Id == id);
        if (detail is null) return NotFound();

        var oldPrice = detail.Price;

        _mapper.Map(flightDetailForUpdate, detail);

        try
        {
            await _context.SaveChangesAsync();

            if (oldPrice != detail.Price)
            {
                _logger.LogInformation("Price changed - Place message on bus.");

                var message = new MessageForNotification
                {
                    FlightCode = detail.Code,
                    OldPrice = oldPrice,
                    NewPrice = detail.Price,
                    WebhookType = "PriceChange",
                };
                _messageBus.SendMessage(message);
            }
            else _logger.LogInformation("No price change.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}