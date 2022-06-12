namespace EM.Web.Models.ViewModels.Venues
{
    public class VenueDetailsViewModel : ICustomMap
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Address { get; set; } = default!;

        public string CityName { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public IEnumerable<VenueEventViewModel> UpcomingEvents { get; set; }
            = Enumerable.Empty<VenueEventViewModel>();

        public void ConfigureMap(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Venue, VenueDetailsViewModel>()
                .ForMember(x => x.UpcomingEvents,
                           x => x.MapFrom(y => y.Events
                                    .AsQueryable()
                                    .Where(x => x.EventDate > DateTime.UtcNow)
                                    .OrderBy(z => z.EventDate)));
        }
    }
}
