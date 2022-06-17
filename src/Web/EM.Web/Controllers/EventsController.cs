namespace EM.Web.Controllers
{
    using EM.Common;
    using EM.Services.Booking.Events;
    using EM.Web.Models.InputModels.Events;
    using EM.Web.Models.ViewModels.Events;
    using EM.Web.Models.ViewModels.Pagination;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static EM.Common.RoleConstants;

    public class EventsController : Controller
    {
        private readonly IEventService eventService;

        public EventsController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> All(int page = 1, int perPage = 5, string? searchQuery = null)
        {
            if (perPage <= 0 || perPage > 20)
            {
                return this.RedirectToAction(nameof(this.All), new { page, perPage = 10 });
            }

            var pageCount = await this.eventService.GetEventsPageCountAsync(perPage, searchQuery);

            if (page <= 0 || (pageCount != 0 && page > pageCount))
            {
                return this.RedirectToAction(nameof(this.All), new { page = 1, perPage, });
            }

            var events = await this.eventService.GetEventsAsync<EventListViewModel>(page, perPage, searchQuery);

            var model = new PaginationViewModel<EventListViewModel>
            {
                Items = events,
                PageCount = pageCount,
                CurrentPage = page,
                DisplayPageCount = GlobalConstants.PaginationDisplayPages,
                ItemsPerPage = perPage,
                SearchQuery = searchQuery,
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await this.eventService.GetEventAsync<EventDetailsViewModel>(id);

            if (model is null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        [HttpGet]
        [Authorize(Roles = Admin)]
        public IActionResult Add(Guid id)
        {
            var model = new EventInputModel
            {
                VenueId = id,
                Tickets = new TicketInputModel[1],
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(EventInputModel eventInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(eventInputModel);
            }

            var eventId = await this.eventService.AddEventAsync(eventInputModel, eventInputModel.Image);

            return this.RedirectToAction(nameof(this.Details), new { Id = eventId });
        }
    }
}
