namespace EM.Web.Models.InputModels.Tickets
{
    using EM.Services.Carts;

    public class TicketInputModel : CartItem
    {
        [Required]
        public override Guid Id { get; set; }

        [Required]
        [Range(1, short.MaxValue)]
        public override short Quantity { get; set; }
    }
}
