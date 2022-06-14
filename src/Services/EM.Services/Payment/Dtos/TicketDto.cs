namespace EM.Services.Payment.Dtos
{
    using AutoMapper;

    using EM.Services.Mapping.Core;

    public class TicketDto : ICustomMap
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public decimal Price { get; set; }

        public void ConfigureMap(IProfileExpression configuration)
        {
            configuration.CreateMap<Ticket, TicketDto>()
                .ForMember(x => x.Name, x => x.MapFrom(y => string.Format("{0} ticket at {1}", y.Type, y.Event.Name)));
        }
    }
}
