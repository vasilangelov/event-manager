namespace EM.Services.Booking.Venues
{
    using EM.Web.Models.InputModels;

    public interface IVenueService
    {
        Task AddVenueAsync(VenueInputModel venueInputModel);
    }
}
