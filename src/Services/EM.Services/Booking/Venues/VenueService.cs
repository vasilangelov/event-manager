namespace EM.Services.Booking.Venues
{
    using AutoMapper;

    using EM.Services.Cities;
    using EM.Services.Cloudinary;

    using Microsoft.AspNetCore.Http;

    using static EM.Common.GlobalConstants;

    [TransientService]
    public class VenueService : IVenueService
    {
        private readonly IRepository<Venue> venueRepository;
        private readonly ICloudinaryService cloudinaryService;
        private readonly ICityService cityService;
        private readonly IMapper mapper;

        public VenueService(IRepository<Venue> venueRepository,
                            ICloudinaryService cloudinaryService,
                            ICityService cityService,
                            IMapper mapper)
        {
            this.venueRepository = venueRepository;
            this.cloudinaryService = cloudinaryService;
            this.cityService = cityService;
            this.mapper = mapper;
        }

        public async Task<T?> GetVenueAsync<T>(Guid id)
            => await this.venueRepository
                            .AllAsNoTracking()
                            .Where(x => x.Id == id)
                            .ProjectTo<T>(this.mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> GetVenuesAsync<T>(int page, int itemsPerPage, string? searchQuery = null)
        {
            var query = this.venueRepository.AllAsNoTracking();

            if (searchQuery is not null)
            {
                query = query.Where(x => x.Name.Contains(searchQuery) ||
                                         x.City.Name.Contains(searchQuery));
            }

            return await query
                            .Skip((page - 1) * itemsPerPage)
                            .Take(itemsPerPage)
                            .ProjectTo<T>(this.mapper.ConfigurationProvider)
                            .ToArrayAsync();
        }

        public async Task<int> GetVenuesPageCountAsync(int itemsPerPage, string? searchQuery = null)
        {
            var query = this.venueRepository.AllAsNoTracking();

            if (searchQuery is not null)
            {
                query = query.Where(x => x.Name.Contains(searchQuery) ||
                                         x.City.Name.Contains(searchQuery));
            }

            var pageCount = await query.CountAsync();

            return (int)Math.Ceiling(pageCount / (double)itemsPerPage);
        }

        public async Task<Guid> AddVenueAsync<T>(T inputModel, IFormFile image, string cityName)
        {
            var venue = this.mapper.Map<Venue>(inputModel);

            venue.CityId = await this.cityService.GetOrCreateCityAsync(cityName);

            venue.ImageUrl = await this.cloudinaryService.UploadImageAsync(Guid.NewGuid().ToString(),
                                                                           VenueImagesFolder,
                                                                           image);

            await this.venueRepository.AddAsync(venue);
            await this.venueRepository.SaveChangesAsync();

            return venue.Id;
        }
    }
}
