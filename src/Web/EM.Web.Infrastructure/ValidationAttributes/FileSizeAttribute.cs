namespace EM.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using static EM.Web.Infrastructure.ValidationAttributes.Common.ExceptionMessages;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FileSizeAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} must not be more than {1}.";

        private readonly long sizeInBytes;

        public FileSizeAttribute(long sizeInBytes)
        {
            this.sizeInBytes = sizeInBytes;
            this.ErrorMessage ??= DefaultErrorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > this.sizeInBytes)
                {
                    var normalizedFileSize = NormalizeFileSize(this.sizeInBytes);

                    return new ValidationResult(string.Format(this.ErrorMessage!,
                                                              validationContext.DisplayName,
                                                              normalizedFileSize));
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(string.Format(InvalidPropertyTypeErrorMessage, validationContext.MemberName, nameof(IFormFile)));
        }

        private static string NormalizeFileSize(long sizeInBytes)
        {
            int trailingZeroTriplets = (int)Math.Log10(sizeInBytes) / 3;

            var fileSize = (short)(sizeInBytes / (Math.Pow(1000, trailingZeroTriplets)));
            var sizeUnit = trailingZeroTriplets switch
            {
                0 => "B",
                1 => "KB",
                2 => "MB",
                3 => "GB",
                4 => "TB",
                _ => throw new NotSupportedException(),
            };

            return $"{fileSize} {sizeUnit}";
        }
    }
}
