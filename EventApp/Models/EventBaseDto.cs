using System.ComponentModel.DataAnnotations;

namespace EventApp.Models;

public abstract class EventBaseDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Category { get; set; } = null!;

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [StringLength(100)]
    public string Location { get; set; } = null!;

    [Required]
    public string Status { get; set; } = "Active";

    [Range(0, 100)]
    public int Progress { get; set; }

    [Range(0, 100000)]
    public decimal Price { get; set; }

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

}
    