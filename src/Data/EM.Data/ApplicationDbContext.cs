namespace EM.Data
{
    using EM.Data.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<City> Cities { get; set; } = default!;

        public DbSet<Venue> Venues { get; set; } = default!;

        public DbSet<Event> Events { get; set; } = default!;

        public DbSet<Ticket> Tickets { get; set; } = default!;

        public DbSet<TicketPurchase> TicketPurchases { get; set; } = default!;

        public DbSet<PurchaseTransaction> PurchaseTransactions { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(builder);
        }
    }
}
