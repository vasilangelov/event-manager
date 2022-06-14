
namespace EM.Services.Payment
{
    using EM.Services.Payment.Models;

    using Stripe.Checkout;

    public interface IStripeService
    {
        Task<Session> CreatePaymentSessionAsync(string successUrl, string cancelUrl);

        Task<SessionResult> GetSessionResultAsync(string sessionId);
    }
}
