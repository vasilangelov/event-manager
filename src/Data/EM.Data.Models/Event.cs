namespace EM.Data.Models
{
    public class Event : AuditableEntity
    {
        public Guid Id { get; set; }

        [MaxLength(40)]
        public string Name { get; set; } = default!;

        public DateTime EventDate { get; set; }

        [MaxLength(300)]
        public string? AdditionalInfo { get; set; } = default!;

        [MaxLength(255)]
        public string ImageUrl { get; set; } = default!;

        public Guid VenueId { get; set; }

        public virtual Venue Venue { get; set; } = default!;

        public virtual ICollection<Ticket> Tickets { get; set; }
            = new HashSet<Ticket>();
    }
}
