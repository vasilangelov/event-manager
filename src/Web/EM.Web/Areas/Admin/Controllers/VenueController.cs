namespace EM.Web.Areas.Dashboard.Controllers
{
    using EM.Services.Booking.Venues;
    using EM.Web.Models.InputModels;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static EM.Common.RoleConstants;

    [Area("Admin")]
    [Authorize(Roles = Admin)]
    public class VenueController : Controller
    {
        private readonly IVenueService venueService;

        public VenueController(IVenueService venueService)
        {
            this.venueService = venueService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(VenueInputModel venueInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(venueInputModel);
            }

            await this.venueService.AddVenueAsync(venueInputModel);

            return this.RedirectToAction("Index", "Home", new { Area = "Admin" });
        }
    }
}
