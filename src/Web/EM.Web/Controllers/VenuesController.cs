namespace EM.Web.Controllers
{
    using EM.Common;
    using EM.Services.Booking.Venues;
    using EM.Web.Models.InputModels.Venues;
    using EM.Web.Models.ViewModels.Pagination;
    using EM.Web.Models.ViewModels.Venues;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static EM.Common.RoleConstants;
    public class VenuesController : Controller
    {
        private readonly IVenueService venueService;

        public VenuesController(IVenueService venueService)
        {
            this.venueService = venueService;
        }

        [HttpGet]
        public async Task<IActionResult> All(int page = 1, int perPage = 10, string? searchQuery = null)
        {
            if (perPage <= 0 || perPage > 20)
            {
                return this.RedirectToAction(nameof(this.All), new { page, perPage = 10 });
            }

            var pageCount = await this.venueService.GetVenuesPageCountAsync(perPage, searchQuery);

            if (page <= 0 || (pageCount != 0 && page > pageCount))
            {
                return this.RedirectToAction(nameof(this.All), new { page = 1, perPage, });
            }

            var venues = await this.venueService.GetVenuesAsync<VenueListViewModel>(page, perPage, searchQuery);

            this.ViewData["SearchQuery"] = searchQuery;

            var model = new PaginationViewModel<VenueListViewModel>
            {
                Items = venues,
                PageCount = pageCount,
                CurrentPage = page,
                DisplayPageCount = GlobalConstants.PaginationDisplayPages,
                ItemsPerPage = perPage,
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await this.venueService.GetVenueAsync<VenueDetailsViewModel>(id);

            if (model is null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        [HttpGet]
        [Authorize(Roles = Admin)]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(VenueInputModel venueInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(venueInputModel);
            }

            var venueId = await this.venueService.AddVenueAsync(venueInputModel, venueInputModel.Image, venueInputModel.CityName);

            return this.RedirectToAction(nameof(this.Details), new { Id = venueId });
        }
    }
}
