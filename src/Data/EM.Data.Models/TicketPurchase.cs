namespace EM.Data.Models
{
    public class TicketPurchase
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; } = default!;

        public Guid TicketId { get; set; }

        public virtual Ticket Ticket { get; set; } = default!;

        public short Amount { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public Guid TransactionId { get; set; } = default!;

        public virtual PurchaseTransaction Transaction { get; set; } = default!;
    }
}
