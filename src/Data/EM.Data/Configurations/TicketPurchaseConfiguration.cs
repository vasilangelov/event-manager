namespace EM.Data.Configurations
{
    using EM.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class TicketPurchaseConfiguration : IEntityTypeConfiguration<TicketPurchase>
    {
        public void Configure(EntityTypeBuilder<TicketPurchase> builder)
        {
            builder
                .HasKey(tp => new { tp.UserId, tp.TicketId });
        }
    }
}
