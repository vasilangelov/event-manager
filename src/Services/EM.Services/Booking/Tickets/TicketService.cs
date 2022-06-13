namespace EM.Services.Booking.Tickets
{
    [TransientService]
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> ticketRepository;
        private readonly IMapper mapper;

        public TicketService(IRepository<Ticket> ticketRepository, IMapper mapper)
        {
            this.ticketRepository = ticketRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<T>> GetTickets<T>(IEnumerable<Guid> ids)
            => await this.ticketRepository
                            .AllAsNoTracking()
                            .Where(x => ids.Contains(x.Id))
                            .ProjectTo<T>(this.mapper.ConfigurationProvider)
                            .ToArrayAsync();
    }
}
