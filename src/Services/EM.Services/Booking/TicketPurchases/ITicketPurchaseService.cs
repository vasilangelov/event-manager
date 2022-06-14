namespace EM.Services.Booking.TicketPurchases
{
    public interface ITicketPurchaseService
    {
        Task<bool> IsTransactionFinished(string sessionId);

        Task AddPurchasedTicketsAsync(Guid userId, string sessionId);
    }
}
