namespace EM.Data.Configurations
{
    using EM.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static EM.Common.GlobalConstants;

    internal class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder
                .HasIndex(c => c.Name);

            builder
                .Property(c => c.Name)
                .UseCollation(CaseInsensitiveDefaultCollation);

            builder
                .Property(c => c.Name)
                .HasMaxLength(50);
        }
    }
}
