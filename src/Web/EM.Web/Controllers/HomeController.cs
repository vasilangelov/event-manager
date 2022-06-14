namespace EM.Web.Controllers
{
    using System.Diagnostics;

    using EM.Services.Booking.Events;
    using EM.Web.Models.ViewModels;
    using EM.Web.Models.ViewModels.Events;

    using Microsoft.AspNetCore.Mvc;

    using static EM.Common.GlobalConstants;

    public class HomeController : Controller
    {
        private readonly IEventService eventService;

        public HomeController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet]
        [ResponseCache(Duration = HomePageResponseCacheDuration, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Index()
        {
            var model = await this.eventService.GetLatestEventsAsync<EventDisplayViewModel>(3);

            return this.View(model);
        }

        public IActionResult PageNotFound()
        {
            return this.View();
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
