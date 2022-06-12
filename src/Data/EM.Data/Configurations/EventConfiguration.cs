namespace EM.Data.Configurations
{
    using EM.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static EM.Common.GlobalConstants;

    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
                .HasIndex(e => e.Name);

            builder
                .Property(e => e.Name)
                .UseCollation(CaseInsensitiveDefaultCollation);

            builder
                .Property(e => e.EventDate)
                .HasConversion(ed => ed.ToUniversalTime(),
                               ed => DateTime.SpecifyKind(ed, DateTimeKind.Utc));

            builder
                .Property(e => e.CreatedAt)
                .HasConversion(ca => ca.ToUniversalTime(),
                               ca => DateTime.SpecifyKind(ca, DateTimeKind.Utc));
        }
    }
}
