namespace EM.Services.Booking.Tickets
{
    using EM.Services.Carts;

    public interface ITicketService
    {
        Task<IEnumerable<T>> GetTicketsAsync<T>(IEnumerable<Guid> ids);

        Task<bool> AllTicketsCanBeBoughtAsync(IDictionary<Guid, CartItem> ticketsInCart);
    }
}
