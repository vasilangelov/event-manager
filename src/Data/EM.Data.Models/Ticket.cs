namespace EM.Data.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }

        [MaxLength(20)]
        public string Type { get; set; } = default!;

        public short Count { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public Guid EventId { get; set; }

        public virtual Event Event { get; set; } = default!;

        public virtual ICollection<TicketPurchase> TicketPurchases { get; set; }
            = new HashSet<TicketPurchase>();
    }
}
