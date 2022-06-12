namespace EM.Data.Configurations
{
    using EM.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static EM.Common.GlobalConstants;

    internal class VenueConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            builder
                .HasIndex(v => v.Name);

            builder
                .Property(v => v.Name)
                .UseCollation(CaseInsensitiveDefaultCollation);
        }
    }
}
