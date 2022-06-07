namespace EM.Services.Booking.Venues
{
    using EM.Data.Infrastructure.Repositories;
    using EM.Data.Models;
    using EM.Services.Cities;
    using EM.Services.Cloudinary;
    using EM.Web.Models.InputModels;

    using static EM.Common.GlobalConstants;

    [TransientService]
    public class VenueService : IVenueService
    {
        private readonly IRepository<Venue> venueRepository;
        private readonly ICloudinaryService cloudinaryService;
        private readonly ICityService cityService;

        public VenueService(IRepository<Venue> venueRepository,
                            ICloudinaryService cloudinaryService,
                            ICityService cityService)
        {
            this.venueRepository = venueRepository;
            this.cloudinaryService = cloudinaryService;
            this.cityService = cityService;
        }

        public async Task AddVenueAsync(VenueInputModel venueInputModel)
        {
            int cityId = await this.cityService.GetOrCreateCityAsync(venueInputModel.City);

            string imageUrl = await this.cloudinaryService.UploadImageAsync(Guid.NewGuid().ToString(),
                                                                            VenueImagesFolder,
                                                                            venueInputModel.Image);

            var venue = new Venue
            {
                Name = venueInputModel.Name,
                Address = venueInputModel.Address,
                CityId = cityId,
                ImageUrl = imageUrl,
            };

            await this.venueRepository.AddAsync(venue);
            await this.venueRepository.SaveChangesAsync();
        }
    }
}
