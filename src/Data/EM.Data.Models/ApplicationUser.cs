namespace EM.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser<Guid>
    {
        public virtual ICollection<TicketPurchase> TicketPurchases { get; set; }
            = new HashSet<TicketPurchase>();
    }
}
