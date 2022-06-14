namespace EM.Web.Models.ViewModels.PurchasedTickets
{
    public class PurchasedTicketsViewModel : ICustomMap
    {
        public string EventName { get; set; } = default!;

        public string EventImageUrl { get; set; } = default!;

        public Guid EventId { get; set; }

        public string VenueName { get; set; } = default!;

        public DateTime EventDate { get; set; }

        public short Amount { get; set; }

        public decimal Price { get; set; }

        public Guid TransactionId { get; set; }

        public void ConfigureMap(IProfileExpression configuration)
        {
            configuration
                .CreateMap<TicketPurchase, PurchasedTicketsViewModel>()
                .ForMember(x => x.EventName, x => x.MapFrom(y => y.Ticket.Event.Name))
                .ForMember(x => x.EventId, x => x.MapFrom(y => y.Ticket.Event.Id))
                .ForMember(x => x.EventImageUrl, x => x.MapFrom(y => y.Ticket.Event.ImageUrl))
                .ForMember(x => x.EventDate, x => x.MapFrom(y => y.Ticket.Event.EventDate))
                .ForMember(x => x.VenueName, x => x.MapFrom(y => y.Ticket.Event.Venue.Name));
        }
    }
}
