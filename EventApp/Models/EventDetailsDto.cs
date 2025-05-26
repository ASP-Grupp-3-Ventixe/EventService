using EventApp.Enums;

namespace EventApp.Models;

public class EventDetailsDto
{   
    public int EventId { get; set; } 
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Location { get; set; } 
    public string? Description { get; set; }
    public EventCategory Category { get; set; }

    public int TicketsSold { get; set; }
    public int MaxTickets { get; set; }
    public decimal PriceFrom { get; set; }
    public string? ImageUrl { get; set; }


    public List<PackageDto> Packages { get; set; } = [];


}
