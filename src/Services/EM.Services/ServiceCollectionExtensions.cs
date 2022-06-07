namespace EM.Services
{
    using EM.Services.Infrastructure;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection serviceCollection)
           => serviceCollection
               .Scan(scan => scan.FromAssemblyOf<TransientServiceAttribute>()
                                 .AddClasses(classes => classes
                                       .WithAttribute(typeof(TransientServiceAttribute)))
                                 .AsImplementedInterfaces()
                                 .WithTransientLifetime());
    }
}
