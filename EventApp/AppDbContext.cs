using EventApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventApp;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<EventEntity> Events { get; set; }
    public DbSet<PackageEntity> Packages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EventEntity>()
            .HasMany(e => e.Packages)
            .WithOne(p => p.Event)
            .HasForeignKey(p => p.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EventEntity>()
       .Property(e => e.Category)
       .HasConversion<string>();

    }

}



