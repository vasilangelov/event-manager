namespace EM.Web.Models.ViewModels.Events
{
    public class EventTicketsViewModel : IMapFrom<Ticket>
    {
        public Guid Id { get; set; }

        public string Type { get; set; } = default!;

        public short Count { get; set; }

        public decimal Price { get; set; }
    }
}
