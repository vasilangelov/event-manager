namespace EM.Data.Seeding
{
    using EM.Data.Models;
    using EM.Data.Seeding.Abstractions;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationRoleSeeder : ISeeder
    {
        private static readonly string[] roleNames = new[] { "Admin" };

        private readonly RoleManager<ApplicationRole> roleManager;

        public ApplicationRoleSeeder(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            bool rolesExist = await this.roleManager.Roles.AnyAsync();

            if (rolesExist)
            {
                return;
            }

            foreach (var roleName in roleNames)
            {
                await this.roleManager.CreateAsync(
                    new ApplicationRole
                    {
                        Name = roleName,
                    });
            }
        }
    }
}
