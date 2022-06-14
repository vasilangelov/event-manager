namespace EM.Web.Extensions
{
    using CloudinaryDotNet;

    using Stripe;

    public static class ServiceCollectionExtensions
    {
        public static void AddCloudinary(this IServiceCollection services, IConfiguration configuration)
        {
            var account = new CloudinaryDotNet.Account
            {
                ApiKey = configuration["Cloudinary:ApiKey"],
                ApiSecret = configuration["Cloudinary:ApiSecret"],
                Cloud = configuration["Cloudinary:Cloud"],
            };

            var cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);
        }

        public static void AddStripe(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(sp =>
            {
                return new StripeClient(apiKey: configuration["Stripe:SecretKey"]);
            });
        }
    }
}
