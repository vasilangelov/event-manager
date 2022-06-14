namespace EM.Services.Payment
{
    using EM.Services.Booking.Tickets;
    using EM.Services.Carts;
    using EM.Services.Payment.Dtos;
    using EM.Services.Payment.Models;

    using Stripe;
    using Stripe.Checkout;

    [TransientService]
    public class StripeService : IStripeService
    {
        private const string EmptyCartErrorMessage = "Cart is empty.";

        private readonly StripeClient stripeClient;
        private readonly ICartService cartService;
        private readonly ITicketService ticketService;

        public StripeService(StripeClient stripeClient,
                             ICartService cartService,
                             ITicketService ticketService)
        {
            this.stripeClient = stripeClient;
            this.cartService = cartService;
            this.ticketService = ticketService;
        }

        public async Task<Session> CreatePaymentSessionAsync(string successUrl, string cancelUrl)
        {
            var lineItems = await this.GetLineItemsAsync();

            var options = new SessionCreateOptions
            {
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            var service = new SessionService(this.stripeClient);

            return await service.CreateAsync(options);
        }

        public async Task<SessionResult> GetSessionResultAsync(string sessionId)
        {
            var sessionService = new SessionService(this.stripeClient);

            Session session;

            try
            {
                session = await sessionService.GetAsync(sessionId);
            }
            catch (StripeException)
            {
                return new SessionResult(SessionStatus.NotFound);
            }

            if (session.Status == "complete")
            {
                return new SessionResult(SessionStatus.Complete, session);
            }
            else
            {
                return new SessionResult(SessionStatus.Pending, session);
            }
        }

        private async Task<List<SessionLineItemOptions>> GetLineItemsAsync()
        {
            var cart = this.cartService.Cart;

            if (cart is null)
            {
                throw new InvalidOperationException(EmptyCartErrorMessage);
            }

            var tickets = await this.ticketService.GetTicketsAsync<TicketDto>(cart.Keys);

            return tickets
                        .Select(t => new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)(t.Price * 100),
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = t.Name,
                                    Metadata = new()
                                    {
                                        { "TicketId", t.Id.ToString() }
                                    }
                                },
                            },
                            Quantity = cart[t.Id].Quantity,
                        })
                        .ToList();
        }
    }
}
