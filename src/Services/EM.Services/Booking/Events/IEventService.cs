namespace EM.Services.Booking.Events
{
    using Microsoft.AspNetCore.Http;

    public interface IEventService
    {
        Task<T?> GetEventAsync<T>(Guid id);

        Task<IEnumerable<T>> GetEventsAsync<T>(int page, int itemsPerPage, string? searchQuery = null);

        Task<int> GetEventsPageCountAsync(int itemsPerPage, string? searchQuery = null);

        Task<Guid> AddEventAsync<T>(T inputModel, IFormFile image);
    }
}
