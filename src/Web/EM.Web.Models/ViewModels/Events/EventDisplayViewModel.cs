namespace EM.Web.Models.ViewModels.Events
{
    public class EventDisplayViewModel : IMapFrom<Event>
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public string VenueName { get; set; } = default!;
    }
}
