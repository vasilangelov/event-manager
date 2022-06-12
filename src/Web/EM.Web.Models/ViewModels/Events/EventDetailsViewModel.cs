namespace EM.Web.Models.ViewModels.Events
{
    public class EventDetailsViewModel : IMapFrom<Event>
    {
        public string Name { get; set; } = default!;

        public DateTime EventDate { get; set; }

        public string ImageUrl { get; set; } = default!;

        public string AdditionalInfo { get; set; } = default!;

        public EventVenueViewModel Venue { get; set; } = default!;

        public IEnumerable<EventTicketsViewModel> Tickets { get; set; }
            = Enumerable.Empty<EventTicketsViewModel>();
    }
}
