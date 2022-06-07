namespace EM.Data.Models
{
    public class Venue
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = default!;

        [Required]
        [MaxLength(50)]
        public string Address { get; set; } = default!;

        [Required]
        public int CityId { get; set; }

        public virtual City City { get; set; } = default!;

        [Required]
        [MaxLength(255)]
        public string ImageUrl { get; set; } = default!;
    }
}
