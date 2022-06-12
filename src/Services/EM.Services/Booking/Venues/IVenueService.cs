namespace EM.Services.Booking.Venues
{
    using Microsoft.AspNetCore.Http;

    public interface IVenueService
    {
        Task<T?> GetVenueAsync<T>(Guid id);

        Task<IEnumerable<T>> GetVenuesAsync<T>(int page, int perPage, string? searchQuery = null);

        Task<int> GetVenuesPageCountAsync(int itemsPerPage, string? searchQuery = null);

        Task<Guid> AddVenueAsync<T>(T inputModel, IFormFile image, string cityName);
    }
}
