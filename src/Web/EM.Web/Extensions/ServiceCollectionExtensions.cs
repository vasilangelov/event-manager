namespace EM.Web.Extensions
{
    using CloudinaryDotNet;

    public static class ServiceCollectionExtensions
    {
        public static void AddCloudinary(this IServiceCollection services, ConfigurationManager configuration)
        {
            var account = new Account
            {
                ApiKey = configuration["Cloudinary:ApiKey"],
                ApiSecret = configuration["Cloudinary:ApiSecret"],
                Cloud = configuration["Cloudinary:Cloud"],
            };

            var cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);
        }
    }
}
