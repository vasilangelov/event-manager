namespace EM.Services.Booking.PurchasedTickets
{
    public interface IPurchasedTicketsService
    {
        Task<IEnumerable<T>> GetActivePurchasedTickets<T>(Guid userId);
    }
}
