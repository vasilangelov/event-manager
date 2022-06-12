namespace EM.Web.Models.ViewModels.Venues
{
    public class VenueListViewModel : IMapFrom<Venue>
    {
        public string Id { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Address { get; set; } = default!;

        public string CityName { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;
    }
}
