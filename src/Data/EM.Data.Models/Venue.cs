namespace EM.Data.Models
{
    public class Venue
    {
        public Guid Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; } = default!;

        [MaxLength(50)]
        public string Address { get; set; } = default!;

        public int CityId { get; set; }

        public virtual City City { get; set; } = default!;

        [MaxLength(255)]
        public string ImageUrl { get; set; } = default!;

        public virtual ICollection<Event> Events { get; set; }
            = new HashSet<Event>();
    }
}
