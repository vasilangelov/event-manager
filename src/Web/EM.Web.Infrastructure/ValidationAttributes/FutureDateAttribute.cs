namespace EM.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using static EM.Web.Infrastructure.ValidationAttributes.Common.ExceptionMessages;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FutureDateAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} must be a date in the future.";

        public FutureDateAttribute()
        {
            this.ErrorMessage ??= DefaultErrorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                date = date.ToUniversalTime();

                if (date > DateTime.UtcNow)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(string.Format(DefaultErrorMessage, validationContext.MemberName));
            }

            return new ValidationResult(string.Format(InvalidPropertyTypeErrorMessage, validationContext.MemberName, nameof(DateTime)));
        }
    }
}
