using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventApp.Models;

public abstract class EventBaseDto
{
    [Required]
    [StringLength(100)]
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(50)]
    [JsonPropertyName("category")]
    public string Category { get; set; } = null!;

    [Required]
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [Required]
    [StringLength(100)]
    [JsonPropertyName("location")]
    public string Location { get; set; } = null!;

    [Required]
    [JsonPropertyName("status")]
    public string Status { get; set; } = "Active";

    [Range(0, 100)]
    [JsonPropertyName("progress")]
    public int Progress { get; set; }

    [Range(0, 100000)]
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [StringLength(1000)]
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
