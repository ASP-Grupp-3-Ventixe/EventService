using EventApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventEntity>().HasData(
            new EventEntity
            {
                Id = 1,
                Title = "Magic",
                Category = "Music",
                Date = DateTime.UtcNow,
                Location = "Hogwarts",
                Status = "Active",
                Progress = 70,
                Price = 120,
                Description = "Time for magic",
                MaxTickets = 2500,
                TicketsSold = 1000
            });
        modelBuilder.Entity<PackageEntity>().HasData(
            new PackageEntity { Id = 1, Name = "VIP", EventId = 1 },
            new PackageEntity { Id = 2, Name = "Diamond", EventId = 1 },
            new PackageEntity { Id = 3, Name = "Platinum", EventId = 1 }
            );
    }
}
