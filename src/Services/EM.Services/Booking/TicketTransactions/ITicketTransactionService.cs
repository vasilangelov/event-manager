namespace EM.Services.Booking.TicketTransactions
{
    public interface ITicketTransactionService
    {
        Task<bool> IsTransactionFinished(string sessionId);

        Task AddPurchasedTicketsAsync(Guid userId, string sessionId);
    }
}
