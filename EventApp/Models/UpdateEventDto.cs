using System.ComponentModel.DataAnnotations;

namespace EventApp.Models;

public class UpdateEventDto : EventBaseDto
{
    [Required]
    public int Id { get; set; } 
}
