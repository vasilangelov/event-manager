namespace EM.Web.Models.ViewModels.Tickets
{
    public class TicketViewModel : ICustomMap
    {
        public Guid Id { get; set; }

        public string Type { get; set; } = default!;

        public string EventName { get; set; } = default!;

        public short Quantity { get; set; }

        public decimal Price { get; set; }

        public void ConfigureMap(IProfileExpression configuration)
        {
            configuration.CreateMap<Ticket, TicketViewModel>();
        }
    }
}
