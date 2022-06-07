namespace EM.Data.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = default!;

        [Required]
        [MaxLength(50)]
        public string NormalizedName { get; set; } = default!;

        public virtual ICollection<Venue> Venues { get; set; }
            = new HashSet<Venue>();
    }
}
