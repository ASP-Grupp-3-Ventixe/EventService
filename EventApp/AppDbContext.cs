using EventApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventApp;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<EventEntity> Events { get; set; }
}



