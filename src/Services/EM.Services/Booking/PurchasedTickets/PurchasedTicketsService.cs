namespace EM.Services.Booking.PurchasedTickets
{
    using static EM.Common.GlobalConstants;

    [TransientService]
    public class PurchasedTicketsService : IPurchasedTicketsService
    {
        private readonly IRepository<TicketPurchase> ticketPurchaseRepository;
        private readonly IMapper mapper;

        public PurchasedTicketsService(IRepository<TicketPurchase> ticketPurchaseRepository,
                                       IMapper mapper)
        {
            this.ticketPurchaseRepository = ticketPurchaseRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<T>> GetActivePurchasedTickets<T>(Guid userId)
        {
            DateTime nowWithOffset = DateTime.UtcNow.Subtract(DisplayTicketAfterExpiration);

            return await this.ticketPurchaseRepository
                                       .AllAsNoTracking()
                                       .Where(x => x.UserId == userId &&
                                                   x.Ticket.Event.EventDate >= nowWithOffset)
                                       .ProjectTo<T>(this.mapper.ConfigurationProvider)
                                       .ToArrayAsync();
        }
    }
}
