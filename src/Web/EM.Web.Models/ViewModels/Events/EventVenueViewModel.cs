namespace EM.Web.Models.ViewModels.Events
{
    public class EventVenueViewModel : IMapFrom<Venue>
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Address { get; set; } = default!;

        public string CityName { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;
    }
}
