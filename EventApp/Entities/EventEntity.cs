namespace EventApp.Entities;

public class EventEntity
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
    public int TicketsSold { get; set; } = 0;
    public string? ImageFileName { get; set; }
    public int MaxTickets { get; set; } = 0;

    public ICollection<PackageEntity> Packages { get; set; } = [];
}
