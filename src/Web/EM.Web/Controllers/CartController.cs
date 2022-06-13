namespace EM.Web.Controllers
{
    using EM.Common;
    using EM.Services.Booking.Tickets;
    using EM.Web.Extensions;
    using EM.Web.Models.InputModels.Tickets;
    using EM.Web.Models.ViewModels.Tickets;

    using Microsoft.AspNetCore.Mvc;

    public class CartController : Controller
    {
        private readonly ITicketService ticketService;

        public CartController(ITicketService ticketService)
        {
            this.ticketService = ticketService;
        }

        [HttpPost]
        public IActionResult Add([Bind(Prefix = null)] TicketInputModel ticket)
        {
            if (!this.ModelState.IsValid)
            {
                TempData["Error"] = "Could not add item to cart";

                return this.Redirect(Request.Headers["Referer"].ToString());
            }

            var cart = this.HttpContext.Session.Get<ICollection<TicketInputModel>>(GlobalConstants.CartSessionKey);

            if (cart is null)
            {
                cart = new List<TicketInputModel>
                {
                    ticket
                };
            }
            else
            {
                cart.Add(ticket);
            }

            this.HttpContext.Session.Set(GlobalConstants.CartSessionKey, cart);

            TempData["Success"] = "Items added to cart successfully";

            return this.Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public IActionResult Remove(Guid id)
        {
            var cart = this.HttpContext.Session.Get<ICollection<TicketInputModel>>(GlobalConstants.CartSessionKey);

            if (cart is not null)
            {
                var ticket = cart.FirstOrDefault(x => x.Id == id);

                if (ticket is not null)
                {
                    cart.Remove(ticket);

                    this.HttpContext.Session.Set(GlobalConstants.CartSessionKey, cart);
                }
            }

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var cartInfo = this.HttpContext.Session.Get<IEnumerable<TicketInputModel>>(GlobalConstants.CartSessionKey);

            if (cartInfo is null)
            {
                return this.View();
            }

            var cartIds = cartInfo?.Select(x => x.Id).ToArray();


            var model = await this.ticketService.GetTickets<TicketViewModel>(cartIds);

            foreach (var element in model)
            {
                element.Quantity = cartInfo!.First(x => x.Id == element.Id).Quantity;
            }

            return this.View(model);
        }
    }
}
