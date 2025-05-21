using EventApp.Entities;
using EventApp.Models;
using EventApp.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventApp.Tests;

public class EventServiceTests
{
    private readonly EventService _eventService;
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;

    public EventServiceTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(_dbContextOptions);

        context.Events.AddRange(
            new EventEntity { Title = "Test 1", Category = "Concert", Date = DateTime.Now, Location = "Stockholm", Status = "Active", Progress = 20, Price = 200, Description = "Testing", TicketsSold = 500 },
            new EventEntity { Title = "Test 2", Category = "Kids", Date = DateTime.Now, Location = "Göteborg", Status = "Active", Progress = 50, Price = 500, Description = "Testing", TicketsSold = 1000 });
        context.SaveChanges();

        var logger = new LoggerFactory().CreateLogger<EventService>();

        _eventService = new EventService(context, logger);
    }

    [Fact]
    public async Task CreateAsync__Should__AddEvent()
    {
        // Arrange
        var newEvent = new CreateEventDto
        {
            Title = "Test 1",
            Category = "Concert",
            Date = DateTime.Now,
            Location = "Stockholm",
            Status = "Active",
            Progress = 20,
            Price = 200,
            Description = "Testing",
        };

        // Act
        var result = await _eventService.CreateAsync(newEvent);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllAsync__Should__ReturnAllEvents()
    {

        // Act
        var result = await _eventService.GetAllAsync();

        // Assert
        result.Count().Should().Be(2);

    }

    [Fact]
    public async Task GetByIdAsync__Should__ReturnCorrectEvent()
    {
            
        // Act
        var result = await _eventService.GetByIdAsync(1);

        // Assert
        result!.Title.Should().Be("Test 1");

    }


    [Fact]
    public async Task UpdateAsync__Should__UpdateEvent()
    {
        // Arrange
        // Arrange
        var existing = new UpdateEventDto
        {
            Title = "Test 1",
            Category = "Concert",
            Date = DateTime.Now,
            Location = "Stockholm",
            Status = "Active",
            Progress = 20,
            Price = 200,
            Description = "Testing",
        };


        // Act
        var result = await _eventService.UpdateAsync(existing);
        var updated = await _eventService.GetByIdAsync(1);


        // Assert
        result.Should().BeTrue();
        updated!.Title.Should().Be("Updated Title", updated.Title);
    }


    [Fact]
    public async Task DeleteAsync__Should__RemoveEvent()
    {

        // Act
        var result = await _eventService.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();


    }







}
