namespace EM.Services.Booking.TicketTransactions
{
    using Stripe;
    using Stripe.Checkout;

    using EM.Data.Infrastructure.Transactions;
    using System.Data;

    [TransientService]
    public class TicketTransactionService : ITicketTransactionService
    {
        private readonly IRepository<TicketPurchase> ticketPurchaseRepository;
        private readonly IRepository<PurchaseTransaction> purchaseTransactionRepository;
        private readonly IRepository<Ticket> ticketRepository;
        private readonly ITransactionManager transactionManager;
        private readonly StripeClient stripeClient;

        public TicketTransactionService(IRepository<TicketPurchase> ticketPurchaseRepository,
                                        IRepository<PurchaseTransaction> purchaseTransactionRepository,
                                        IRepository<Ticket> ticketRepository,
                                        ITransactionManager transactionManager,
                                        StripeClient stripeClient)
        {
            this.ticketPurchaseRepository = ticketPurchaseRepository;
            this.purchaseTransactionRepository = purchaseTransactionRepository;
            this.ticketRepository = ticketRepository;
            this.transactionManager = transactionManager;
            this.stripeClient = stripeClient;
        }

        public async Task<bool> IsTransactionFinished(string sessionId)
            => await this.purchaseTransactionRepository
                            .AllAsNoTracking()
                            .AnyAsync(x => x.SessionId == sessionId);

        public async Task AddPurchasedTicketsAsync(Guid userId, string sessionId)
        {
            var service = new SessionService(this.stripeClient);

            var listLineItems = await service.ListLineItemsAsync(sessionId, new SessionListLineItemsOptions
            {
                Expand = new List<string>
                {
                    "data.price.product",
                }
            });

            using var transaction = await this.transactionManager.BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                var purchaseTransaction = new PurchaseTransaction
                {
                    SessionId = sessionId,
                };

                foreach (var item in listLineItems)
                {
                    Guid ticketId = Guid.Parse(item.Price.Product.Metadata["TicketId"]);

                    var ticketPurchase = new TicketPurchase
                    {
                        Amount = (short)item.Quantity!,
                        Price = (decimal)item.AmountTotal! / 100m,
                        TicketId = ticketId,
                        UserId = userId,
                        Transaction = purchaseTransaction,
                    };

                    await this.ticketPurchaseRepository.AddAsync(ticketPurchase);

                    var ticket = await this.ticketRepository.FindByIdAsync(ticketId);

                    if (ticket is not null)
                    {
                        ticket.Count -= (short)item.Quantity!;
                    }
                }

                await this.ticketPurchaseRepository.SaveChangesAsync();

                await transaction.CommitAsync();
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
