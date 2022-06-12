namespace EM.Data.Models
{
    public class City
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = default!;

        public virtual ICollection<Venue> Venues { get; set; }
            = new HashSet<Venue>();
    }
}
