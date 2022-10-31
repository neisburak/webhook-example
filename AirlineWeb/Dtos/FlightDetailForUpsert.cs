namespace AirlineWeb.Dtos;

public class FlightDetailForUpsert
{
    public string Code { get; set; } = default!;
    public decimal Price { get; set; }
}