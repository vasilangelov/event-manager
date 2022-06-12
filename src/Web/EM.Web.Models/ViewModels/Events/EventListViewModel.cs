namespace EM.Web.Models.ViewModels.Events
{
    public class EventListViewModel : IMapFrom<Event>
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public DateTime EventDate { get; set; }

        public string VenueName { get; set; } = default!;

        public string VenueAddress { get; set; } = default!;

        public string VenueCityName { get; set; } = default!;
    }
}
