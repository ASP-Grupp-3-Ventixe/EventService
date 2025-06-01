using EventApp;
using EventApp.Entities;
using EventApp.Enums;
using EventApp.Models;
using EventApp.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventApp.Tests;

// Denna testfil är genererad av ChatGPT enligt mina instruktioner och min kodstruktur.

public class EventServiceTests
{
    private readonly EventService _eventService;
    private readonly AppDbContext _context;

    public EventServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        var logger = new LoggerFactory().CreateLogger<EventService>();
        var configuration = new ConfigurationBuilder().Build();

        _eventService = new EventService(_context, logger, configuration);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_Event()
    {
        // Arrange
        var createDto = new CreateEventDto
        {
            Title = "Concert Event",
            Category = EventCategory.Concert,
            Date = DateTime.UtcNow,
            Location = "Stockholm",
            Status = "Active",
            Progress = 50,
            Price = 199,
            Description = "Awesome concert",
            MaxTickets = 500
        };

        // Act
        var result = await _eventService.CreateAsync(createDto);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Events()
    {
        // Arrange
        await SeedEvent();

        // Act
        var result = await _eventService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Event()
    {
        // Arrange
        var seeded = await SeedEvent();

        // Act
        var result = await _eventService.GetByIdAsync(seeded.Id);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Event()
    {
        // Arrange
        var seeded = await SeedEvent();

        var updateDto = new UpdateEventDto
        {
            Id = seeded.Id,
            Title = "Updated Event",
            Category = EventCategory.Sport,
            Date = DateTime.UtcNow,
            Location = "Gothenburg",
            Status = "Cancelled",
            Progress = 75,
            Price = 499,
            Description = "Updated description",
            MaxTickets = 1000,
            Packages = []
        };

        // Act
        var result = await _eventService.UpdateAsync(updateDto);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Event()
    {
        // Arrange
        var seeded = await SeedEvent();

        // Act
        var result = await _eventService.DeleteAsync(seeded.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IncreaseTicketsSoldAsync_Should_Increase_Tickets()
    {
        // Arrange
        var seeded = await SeedEvent();

        // Act
        var result = await _eventService.IncreaseTicketsSoldAsync(seeded.Id, 50);

        // Assert
        result.Should().BeTrue();
    }

    private async Task<EventEntity> SeedEvent()
    {
        var entity = new EventEntity
        {
            Title = "Seeded Event",
            Category = EventCategory.Concert,
            Date = DateTime.UtcNow,
            Location = "Stockholm",
            Status = "Active",
            Progress = 20,
            Price = 299,
            Description = "Seed description",
            TicketsSold = 0,
            MaxTickets = 500,
            Packages = []
        };

        _context.Events.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
