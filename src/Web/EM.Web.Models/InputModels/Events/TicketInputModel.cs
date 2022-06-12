namespace EM.Web.Models.InputModels.Events
{
    public class TicketInputModel : IMapTo<Ticket>
    {
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Type { get; set; } = default!;

        [Required]
        [Range(1, short.MaxValue)]
        public short Count { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
