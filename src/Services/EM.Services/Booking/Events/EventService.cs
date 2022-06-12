namespace EM.Services.Booking.Events
{
    using EM.Services.Cloudinary;

    using Microsoft.AspNetCore.Http;

    using static EM.Common.GlobalConstants;

    [TransientService]
    public class EventService : IEventService
    {
        private readonly IRepository<Event> eventRepository;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IMapper mapper;

        public EventService(IRepository<Event> eventRepository,
                            ICloudinaryService cloudinaryService,
                            IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.cloudinaryService = cloudinaryService;
            this.mapper = mapper;
        }

        public async Task<T?> GetEventAsync<T>(Guid id)
            => await this.eventRepository
                            .AllAsNoTracking()
                            .Where(x => x.Id == id)
                            .ProjectTo<T>(this.mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> GetEventsAsync<T>(int page, int itemsPerPage, string? searchQuery = null)
        {
            var query = this.eventRepository.AllAsNoTracking().Where(x => x.EventDate >= DateTime.UtcNow);

            if (searchQuery is not null)
            {
                query = query.Where(x => x.Name.StartsWith(searchQuery) ||
                                         x.Venue.Name.StartsWith(searchQuery) ||
                                         x.Venue.City.Name.StartsWith(searchQuery));
            }

            return await query
                            .OrderByDescending(x => x.CreatedAt)
                            .Skip((page - 1) * itemsPerPage)
                            .Take(itemsPerPage)
                            .ProjectTo<T>(this.mapper.ConfigurationProvider)
                            .ToArrayAsync();
        }

        public async Task<int> GetEventsPageCountAsync(int itemsPerPage, string? searchQuery = null)
        {
            var query = this.eventRepository.AllAsNoTracking().Where(x => x.EventDate >= DateTime.UtcNow);

            if (searchQuery is not null)
            {
                query = query.Where(x => x.Name.StartsWith(searchQuery) ||
                                         x.Venue.Name.StartsWith(searchQuery) ||
                                         x.Venue.City.Name.StartsWith(searchQuery));
            }

            var pageCount = await query.CountAsync();

            return (int)Math.Ceiling(pageCount / (double)itemsPerPage);
        }

        public async Task<Guid> AddEventAsync<T>(T inputModel, IFormFile image)
        {
            var eventModel = this.mapper.Map<Event>(inputModel);

            eventModel.ImageUrl = await this.cloudinaryService.UploadImageAsync(Guid.NewGuid().ToString(),
                                                                                EventImagesFolder,
                                                                                image);

            await this.eventRepository.AddAsync(eventModel);
            await this.eventRepository.SaveChangesAsync();

            return eventModel.Id;
        }
    }
}
