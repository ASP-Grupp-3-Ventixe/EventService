using System.ComponentModel.DataAnnotations;

namespace EventApp.Models;

public class EventDto
{
    public int Id { get; set; } 
    public string Title { get; set; } = null!;
    public string Category { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Location { get; set; } = null!;
    public string Status { get; set; } = "Active";
    public int Progress { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public int? TicketsSold { get; set; }
    public string? ImageFileName { get; set; }
    public string? ImageUrl { get; set; }   
}
