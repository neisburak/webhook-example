namespace AirlineWeb.Dtos;

public class FlightDetailForView
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public decimal Price { get; set; }
}