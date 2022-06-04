namespace EM.Data.Models
{
    public class Venue
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Address { get; set; } = default!;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string ImageUrl { get; set; } = default!;
    }
}
