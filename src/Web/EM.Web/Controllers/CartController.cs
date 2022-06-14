namespace EM.Web.Controllers
{
    using EM.Services.Booking.Tickets;
    using EM.Services.Carts;
    using EM.Web.Models.InputModels.Tickets;
    using EM.Web.Models.ViewModels.Tickets;

    using Microsoft.AspNetCore.Mvc;

    public class CartController : Controller
    {
        private readonly ITicketService ticketService;
        private readonly ICartService cartService;

        public CartController(ITicketService ticketService, ICartService cartService)
        {
            this.ticketService = ticketService;
            this.cartService = cartService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add([Bind(Prefix = null)] TicketInputModel ticket)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData["Error"] = "Could not add item to cart";

                return this.Redirect(Request.Headers["Referer"].ToString());
            }

            this.cartService.AddToCart(ticket);

            this.TempData["Success"] = "Items added to cart successfully";

            return this.Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public IActionResult Remove(Guid id)
        {
            this.cartService.RemoveFromCart(id);

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var cart = this.cartService.Cart;

            if (cart is null)
            {
                return this.View();
            }

            var model = await this.ticketService.GetTicketsAsync<TicketViewModel>(cart.Keys);

            foreach (var element in model)
            {
                element.Quantity = cart[element.Id].Quantity;
            }

            return this.View(model);
        }
    }
}
