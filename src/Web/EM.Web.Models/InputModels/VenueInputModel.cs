﻿namespace EM.Web.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using EM.Web.Infrastructure.ValidationAttributes;

    using Microsoft.AspNetCore.Http;

    using static EM.Common.GlobalConstants;

    public class VenueInputModel
    {
        [Required]
        [StringLength(30, MinimumLength = 6)]
        public string Name { get; set; } = default!;

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Address { get; set; } = default!;

        [Required]
        [StringLength(50, MinimumLength = 5)]
        [RegularExpression("^([A-Z][a-z]+\\x20)*([A-Z][a-z]+)$", ErrorMessage = "The names of the {0} must begin with captial letter and continue with small letters.")]
        public string City { get; set; } = default!;

        [Required]
        [FileExtension(".jpeg", ".jpg", ".png")]
        [FileSize(MaxImageSize)]
        public IFormFile Image { get; set; } = default!;
    }
}
