namespace EM.Data.Configurations
{
    using EM.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PurchaseTransactionConfiguration : IEntityTypeConfiguration<PurchaseTransaction>
    {
        public void Configure(EntityTypeBuilder<PurchaseTransaction> builder)
        {
            builder
                .HasIndex(x => x.SessionId)
                .IsUnique();
        }
    }
}
