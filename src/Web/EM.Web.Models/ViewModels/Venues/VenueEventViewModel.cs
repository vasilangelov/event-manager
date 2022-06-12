namespace EM.Web.Models.ViewModels.Venues
{
    public class VenueEventViewModel : IMapFrom<Event>
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public DateTime EventDate { get; set; }
    }
}
