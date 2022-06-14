namespace EM.Data.Models
{
    public class PurchaseTransaction
    {
        public Guid Id { get; set; } = default!;

        public string SessionId { get; set; } = default!;

        public virtual ICollection<TicketPurchase> TicketPurchases { get; set; }
            = new HashSet<TicketPurchase>();
    }
}
