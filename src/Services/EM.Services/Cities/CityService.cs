namespace EM.Services.Cities
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EM.Data.Infrastructure.Repositories;
    using EM.Data.Models;

    using Microsoft.EntityFrameworkCore;

    [TransientService]
    public class CityService : ICityService
    {
        private readonly IRepository<City> cityRepository;

        public CityService(IRepository<City> cityRepository)
        {
            this.cityRepository = cityRepository;
        }

        public async Task<int> GetOrCreateCityAsync(string name)
        {
            int? id = await this.cityRepository
                                    .AllAsNoTracking()
                                    .Where(x => x.NormalizedName == name.ToUpper())
                                    .Select<City, int?>(x => x.Id)
                                    .FirstOrDefaultAsync();

            if (id.HasValue)
            {
                return id.Value;
            }

            return await this.CreateCityAsync(name);
        }

        public async Task<IEnumerable<string>> GetSimilarCityNames(string name, int maxResults)
            => await this.cityRepository
                            .AllAsNoTracking()
                            .Where(x => x.NormalizedName.StartsWith(name.ToUpper()))
                            .Take(maxResults)
                            .Select(x => x.Name)
                            .ToArrayAsync();

        private async Task<int> CreateCityAsync(string name)
        {
            var city = new City
            {
                Name = name,
                NormalizedName = name.ToUpper(),
            };

            await this.cityRepository.AddAsync(city);
            await this.cityRepository.SaveChangesAsync();

            return city.Id;
        }
    }
}
