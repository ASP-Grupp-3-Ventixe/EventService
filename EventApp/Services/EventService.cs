using EventApp.Entities;
using EventApp.Interfaces;
using EventApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Services;

public class EventService(AppDbContext context, ILogger<EventService> logger, IConfiguration configuration) : IEventService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<EventService> _logger = logger;
    private readonly IConfiguration _config = configuration;

    public async Task<bool> CreateAsync(CreateEventDto model)
    {
        try
        {
            var entity = new EventEntity
            {
                Title = model.Title,
                Category = model.Category,
                Date = DateTime.SpecifyKind(model.Date, DateTimeKind.Utc),
                Location = model.Location,
                Status = model.Status,
                Progress = model.Progress,
                Price = model.Price,
                Description = model.Description,
                TicketsSold = 0,
                MaxTickets = model.MaxTickets,
                Packages = model.Packages?.Select(p => new PackageEntity { Name = p.Name, Price = p.Price }).ToList()!
            };

            _context.Events.Add(entity);
            _logger.LogInformation(" CREATE: {@model}", model);

            return await _context.SaveChangesAsync() > 0;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, " CreateAsync failed. Data: {@model}", model);
            throw;
        }
    }

    public async Task<bool> IncreaseTicketsSoldAsync(int eventId, int quantity)
    {
        try
        {
            var entity = await _context.Events.FindAsync(eventId);
            if (entity == null) return false;

            entity.TicketsSold += quantity;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to increase TicketsSold for eventId {EventId}", eventId);
            return false;
        }
    }

    public async Task<IEnumerable<EventDto>> GetAllAsync()
    {
        try
        {
            var imageBaseUrl = _config["ImageBaseUrl"] ?? "https://eventservice-rk6f.onrender.com";

            return await _context.Events
           .Select(e => new EventDto
           {
               Id = e.Id,
               Title = e.Title,
               Category = e.Category,
               Date = e.Date,
               Location = e.Location,
               Status = e.Status,
               Progress = e.Progress,
               Price = e.Price,
               Description = e.Description,
               TicketsSold = e.TicketsSold,
               ImageFileName = e.ImageFileName,
               ImageUrl = string.IsNullOrEmpty(e.ImageFileName)
                   ? null
                   : $"{imageBaseUrl}/event-images/{e.ImageFileName}"
           }).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllAsync failed.");
            return Enumerable.Empty<EventDto>();
        }
    }

    public async Task<EventDto?> GetByIdAsync(int id)
    {
        try
        {
            var e = await _context.Events.FindAsync(id);
            if (e == null) return null;

            var imageBaseUrl = _config["ImageBaseUrl"] ?? "https://eventservice-rk6f.onrender.com";

            return new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Category = e.Category,
                Date = e.Date,
                Location = e.Location,
                Status = e.Status,
                Progress = e.Progress,
                Price = e.Price,
                Description = e.Description,
                TicketsSold = e.TicketsSold,
                ImageFileName = e.ImageFileName,
                ImageUrl = string.IsNullOrEmpty(e.ImageFileName)
                    ? null
                    : $"{imageBaseUrl}/event-images/{e.ImageFileName}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetByIdAsync failed.");
            return null;
        }
    }

    public async Task<EventDetailsDto?> GetEventByIdAsync(int id)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Packages)
            .FirstOrDefaultAsync(e => e.Id == id);


        if (eventEntity == null)
            return null;

        return new EventDetailsDto
        {
            EventId = eventEntity.Id,
            Title = eventEntity.Title,
            Category = eventEntity.Category,
            Status = eventEntity.Status,
            Date = eventEntity.Date,
            Location = eventEntity.Location,
            TicketsSold = eventEntity.TicketsSold,
            MaxTickets = eventEntity.MaxTickets,
            PriceFrom = eventEntity.Price,
            Packages = [.. eventEntity.Packages.Select(p => new PackageDto
            {
                Name = p.Name,
                Price = p.Price,
            })]

        };
    }

    public async Task<bool> UpdateAsync(UpdateEventDto model)
    {
        try
        {
            var entity = await _context.Events.FindAsync(model.Id);
            if (entity == null) return false;

            entity.Title = model.Title;
            entity.Category = model.Category;
            entity.Date = DateTime.SpecifyKind(model.Date, DateTimeKind.Utc);
            entity.Location = model.Location;
            entity.Status = model.Status;
            entity.Progress = model.Progress;
            entity.Price = model.Price;
            entity.Description = model.Description;
            entity.MaxTickets = model.MaxTickets;
            entity.Packages = [.. model.Packages!.Select(p => new PackageEntity { Name = p.Name, Price = p.Price, EventId = model.Id })];

            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateAsync failed.");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = await _context.Events.FindAsync(id);
            if (entity == null) return false;

            _context.Events.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteAsync failed.");
            return false;
        }
    }

    public async Task<(bool success, string? oldFileName)> SaveImageAsync(int eventId, string newFileName)
    {
        try
        {
            var entity = await _context.Events.FindAsync(eventId);
            if (entity == null) return (false, null);

            var oldFileName = entity.ImageFileName;
            entity.ImageFileName = newFileName;

            await _context.SaveChangesAsync();
            return (true, oldFileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SaveImageAsync failed");
            return (false, null);
        }
    }

    public async Task<bool> ReplaceEventImageAsync(int eventId, string newfileName)
    {
        try
        {
            var entity = await _context.Events.FindAsync(eventId);
            if (entity == null)
                return false;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "event-images");

            if (!string.IsNullOrEmpty(entity.ImageFileName))
            {
                var oldPath = Path.Combine(uploadsFolder, entity.ImageFileName);
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }

            entity.ImageFileName = newfileName;
            await _context.SaveChangesAsync();

            return true;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ReplaceEventImageAsync failed.");
            return false;
        }
    }
}
