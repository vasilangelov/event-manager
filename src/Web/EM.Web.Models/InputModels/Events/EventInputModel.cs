namespace EM.Web.Models.InputModels.Events
{
    using Microsoft.AspNetCore.Http;

    using static EM.Common.GlobalConstants;

    public class EventInputModel : IMapTo<Event>
    {
        [Required]
        [StringLength(40, MinimumLength = 5)]
        public string Name { get; set; } = default!;

        [Required]
        [FutureDate]
        public DateTime EventDate { get; set; }

        [Required]
        public Guid VenueId { get; set; }

        [StringLength(300)]
        [Display(Name = "Additional Info")]
        public string? AdditionalInfo { get; set; } = default!;

        [Required]
        [FileExtension(".jpeg", ".jpg", ".png")]
        [FileSize(MaxImageSize)]
        public IFormFile Image { get; set; } = default!;

        [MinLength(1)]
        [MaxLength(8)]
        public TicketInputModel[] Tickets { get; set; }
            = Array.Empty<TicketInputModel>();
    }
}
