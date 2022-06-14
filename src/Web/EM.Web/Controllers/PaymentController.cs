namespace EM.Web.Controllers
{
    using System.Security.Claims;

    using EM.Services.Booking.Tickets;
    using EM.Services.Booking.TicketTransactions;
    using EM.Services.Carts;
    using EM.Services.Payment;
    using EM.Services.Payment.Models;
    using EM.Web.Infrastructure.Extensions;
    using EM.Web.Models.ViewModels.Payment;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static EM.Web.Infrastructure.Helpers.NameHelpers;

    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IStripeService stripeService;
        private readonly ICartService cartService;
        private readonly ITicketService ticketService;
        private readonly ITicketTransactionService ticketPurchaseService;

        public PaymentController(IStripeService stripeService,
                                 ICartService cartService,
                                 ITicketService ticketService,
                                 ITicketTransactionService ticketPurchaseService)
        {
            this.stripeService = stripeService;
            this.cartService = cartService;
            this.ticketService = ticketService;
            this.ticketPurchaseService = ticketPurchaseService;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            if (this.cartService.Cart is null)
            {
                return this.RedirectToAction(nameof(HomeController.Index), GetControllerName<HomeController>());
            }

            bool allTicketsCanBeBought = await this.ticketService.AllTicketsCanBeBoughtAsync(this.cartService.Cart);

            if (!allTicketsCanBeBought)
            {
                this.cartService.ClearCart();

                this.TempData["Error"] = "You cannot buy these tickets.";

                return this.RedirectToAction(nameof(HomeController.Index), GetControllerName<HomeController>());
            }

            string successUrl = this.Url.ActionRaw(protocol: "https",
                                                   controller: GetControllerName<PaymentController>(),
                                                   action: nameof(PaymentController.Result),
                                                   values: new { session_id = "{CHECKOUT_SESSION_ID}" });

            string cancelUrl = this.Url.Action(protocol: "https",
                                               controller: GetControllerName<PaymentController>(),
                                               action: nameof(PaymentController.Cancel),
                                               values: null)!;

            var session = await this.stripeService.CreatePaymentSessionAsync(successUrl, cancelUrl);

            return this.Redirect(session.Url);
        }

        [HttpGet]
        public async Task<IActionResult> Result([FromQuery(Name = "session_id")] string sessionId)
        {
            var isTransactionFinished = await this.ticketPurchaseService.IsTransactionFinished(sessionId);

            if (isTransactionFinished)
            {
                return this.RedirectToAction(nameof(HomeController.Index), GetControllerName<HomeController>());
            }

            var sessionResult = await this.stripeService.GetSessionResultAsync(sessionId);

            PaymentStatusViewModel model;

            if (sessionResult.SessionStatus == SessionStatus.Complete)
            {
                model = new()
                {
                    Success = true,
                };

                var userId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

                await this.ticketPurchaseService.AddPurchasedTicketsAsync(userId, sessionId);

                this.cartService.ClearCart();
            }
            else if (sessionResult.SessionStatus == SessionStatus.NotFound)
            {
                return this.NotFound();
            }
            else
            {
                model = new()
                {
                    Success = false,
                    RetryUrl = sessionResult.Session!.Url,
                };
            }

            return this.View(model);
        }

        [HttpGet]
        public IActionResult Cancel()
        {
            this.TempData["Error"] = "Your payment has been canceled.";

            return this.RedirectToAction("Home", "Index");
        }
    }
}
