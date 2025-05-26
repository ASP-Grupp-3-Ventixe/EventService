using EventApp.Models;

namespace EventApp.Interfaces
{
    public interface IEventService
    {
        Task<bool> CreateAsync(CreateEventDto model);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<EventDto>> GetAllAsync();
        Task<EventDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(UpdateEventDto model);
        Task<(bool success, string? oldFileName)> SaveImageAsync(int eventId, string newFileName);
        Task<bool> ReplaceEventImageAsync(int eventId, string newFileName, string imageUrl);
        Task<EventDetailsDto?> GetEventByIdAsync(int id);
        Task<bool> IncreaseTicketsSoldAsync(int eventId, int quantity);
        Task<UpdateEventDto?> GetEventForEditAsync(int id);

    }   
}           