namespace EM.Web.Extensions
{
    using EM.Data;
    using EM.Data.Seeding;
    using EM.Data.Seeding.Abstractions;

    using Microsoft.EntityFrameworkCore;

    public static class WebApplicationExtensions
    {
        private const string SeederDoesNotInheritFromBaseExceptionMessage = "Seeder type {0} does not inherit from base {1}.";

        public static async Task MigrateDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var serviceProvider = scope.ServiceProvider;

            IEnumerable<Type> seederTypes = new Type[]
            {
                typeof(ApplicationRoleSeeder),
                typeof(ApplicationUserSeeder),
                typeof(CitySeeder),
            };

            EnsureAllSeedersInheritFromBase(seederTypes);

            foreach (var seederType in seederTypes)
            {
                var seederInstance = (ISeeder)ActivatorUtilities.CreateInstance(serviceProvider, seederType);

                await seederInstance.SeedAsync();
            }
        }

        private static void EnsureAllSeedersInheritFromBase(IEnumerable<Type> seederTypes)
        {
            Type? nonInheritedSeeder = seederTypes.FirstOrDefault(s => !s.IsAssignableTo(typeof(ISeeder)));

            if (nonInheritedSeeder is not null)
            {
                throw new InvalidOperationException(string.Format(
                    SeederDoesNotInheritFromBaseExceptionMessage,
                    nonInheritedSeeder.Name,
                    nameof(ISeeder)));
            }
        }
    }
}
