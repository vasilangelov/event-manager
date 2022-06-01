namespace EM.Web.Extensions
{
    using EM.Data;

    using Microsoft.EntityFrameworkCore;

    public static class WebApplicationExtensions
    {
        public static async Task MigrateDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.MigrateAsync();
        }
    }
}
