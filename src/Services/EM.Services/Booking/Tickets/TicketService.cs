namespace EM.Services.Booking.Tickets
{
    using System.Data;

    using EM.Data.Infrastructure.Transactions;
    using EM.Services.Carts;

    [TransientService]
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> ticketRepository;
        private readonly ITransactionManager transactionManager;
        private readonly IMapper mapper;

        public TicketService(IRepository<Ticket> ticketRepository,
                             ITransactionManager transactionManager,
                             IMapper mapper)
        {
            this.ticketRepository = ticketRepository;
            this.transactionManager = transactionManager;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<T>> GetTicketsAsync<T>(IEnumerable<Guid> ids)
            => await this.ticketRepository
                            .AllAsNoTracking()
                            .Where(x => ids.Contains(x.Id))
                            .ProjectTo<T>(this.mapper.ConfigurationProvider)
                            .ToArrayAsync();

        public async Task<bool> AllTicketsCanBeBoughtAsync(IDictionary<Guid, CartItem> ticketsInCart)
        {
            var transaction = await this.transactionManager.BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                var ticketIds = ticketsInCart.Keys;

                var ticketInfo = await this.ticketRepository
                                    .AllAsNoTracking()
                                    .Where(x => ticketIds.Contains(x.Id))
                                    .Select(x => new { x.Id, x.Count })
                                    .ToArrayAsync();

                return ticketInfo
                        .AsParallel()
                        .All(x => x.Count >= ticketsInCart[x.Id].Quantity);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                throw;
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }
    }
}
