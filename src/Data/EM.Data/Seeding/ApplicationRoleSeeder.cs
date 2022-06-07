namespace EM.Data.Seeding
{
    using System.Reflection;

    using EM.Common;
    using EM.Data.Models;
    using EM.Data.Seeding.Abstractions;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationRoleSeeder : ISeeder
    {
        private const string InvalidRoleStructureExceptionMessage = "Type {0} must contain only string constants.";
        private const BindingFlags RolePropertySelector = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;

        private readonly RoleManager<ApplicationRole> roleManager;

        public ApplicationRoleSeeder(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            var rolesExist = await this.roleManager.Roles.AnyAsync();

            if (rolesExist)
            {
                return;
            }

            var roleNames = typeof(RoleConstants)
                .GetFields(RolePropertySelector)
                .Select(x =>
                x.GetRawConstantValue() as string ??
                    throw new InvalidOperationException(string.Format(InvalidRoleStructureExceptionMessage, nameof(RoleConstants))))
                .ToArray();

            foreach (var roleName in roleNames)
            {
                ApplicationRole role = new()
                {
                    Name = roleName,
                };

                await this.roleManager.CreateAsync(role);
            }
        }
    }
}
