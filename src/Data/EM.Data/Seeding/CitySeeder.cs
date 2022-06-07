namespace EM.Data.Seeding
{
    using System.Threading.Tasks;

    using EM.Data.Models;
    using EM.Data.Seeding.Abstractions;

    using Microsoft.EntityFrameworkCore;

    public class CitySeeder : ISeeder
    {
        private static readonly IEnumerable<string> cityNames = new string[]
        {
            "Sofia",
            "Plovdiv",
            "Varna",
            "Burgas",
            "Veliko Tarnovo",
        };

        private readonly ApplicationDbContext dbContext;

        public CitySeeder(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            bool citiesExist = await this.dbContext.Cities.AnyAsync();

            if (citiesExist)
            {
                return;
            }

            var models = cityNames
                            .Select(name => new City
                            {
                                Name = name,
                                NormalizedName = name.ToUpper(),
                            })
                            .ToArray();

            await this.dbContext.AddRangeAsync(models);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
