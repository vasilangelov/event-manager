namespace EM.Web.Controllers
{
    using System.Security.Claims;

    using EM.Services.Booking.PurchasedTickets;
    using EM.Web.Models.ViewModels.PurchasedTickets;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class TicketsController : Controller
    {
        private readonly IPurchasedTicketsService purchasedTicketsService;

        public TicketsController(IPurchasedTicketsService purchasedTicketsService)
        {
            this.purchasedTicketsService = purchasedTicketsService;
        }

        [HttpGet]
        public async Task<IActionResult> Active()
        {
            var userId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = await this.purchasedTicketsService.GetActivePurchasedTickets<PurchasedTicketsViewModel>(userId);

            return this.View(model);
        }
    }
}
