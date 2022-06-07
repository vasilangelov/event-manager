using EM.Data;
using EM.Data.Infrastructure.Repositories;
using EM.Data.Infrastructure.Repositories.EntityFramework;
using EM.Data.Models;
using EM.Services;
using EM.Services.Mapping;
using EM.Web.Extensions;
using EM.Web.Models.ViewModels;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region ConfigureServices
builder.Services
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.User.RequireUniqueEmail = true;

        if (builder.Environment.IsDevelopment())
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        }
    })
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));

builder.Services.AddAutoMapper(typeof(ErrorViewModel).Assembly);

builder.Services.AddCloudinary(builder.Configuration);

builder.Services.AddServiceLayer();
#endregion

var app = builder.Build();

await app.MigrateDatabaseAsync();
await app.SeedDatabaseAsync();

#region Configure
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(routeBuilder =>
{
    routeBuilder.MapControllerRoute("area", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    routeBuilder.MapDefaultControllerRoute();

    routeBuilder.MapRazorPages();
});
#endregion

app.Run();
