using System.ComponentModel.DataAnnotations;

namespace EventApp.Models;

public class CreateEventDto : EventBaseDto
{
    public List<PackageDto>? Packages { get; set; } 
}
    