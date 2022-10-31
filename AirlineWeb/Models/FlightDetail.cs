namespace AirlineWeb.Models;

public class FlightDetail
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public decimal Price { get; set; }
}