namespace EM.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using static EM.Web.Infrastructure.ValidationAttributes.Common.ExceptionMessages;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FileExtensionAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} can not have extension of type {1}. Allowed extensions {2}.";

        private readonly IEnumerable<string> allowedExtensions;

        public FileExtensionAttribute(params string[] allowedExtensions)
        {
            this.ErrorMessage ??= DefaultErrorMessage;
            this.allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (this.allowedExtensions.All(x => x != extension))
                {
                    return new ValidationResult(string.Format(this.ErrorMessage!,
                                                              validationContext.DisplayName,
                                                              extension,
                                                              string.Join(", ", this.allowedExtensions)));
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(string.Format(InvalidPropertyTypeErrorMessage, validationContext.MemberName, nameof(IFormFile)));
        }
    }
}
