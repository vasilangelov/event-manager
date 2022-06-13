namespace EM.Services.Booking.Tickets
{
    public interface ITicketService
    {
        Task<IEnumerable<T>> GetTickets<T>(IEnumerable<Guid> ids);
    }
}
