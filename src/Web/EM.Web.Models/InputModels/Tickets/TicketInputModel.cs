namespace EM.Web.Models.InputModels.Tickets
{
    public class TicketInputModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Range(1, short.MaxValue)]
        public short Quantity { get; set; }
    }
}
