namespace EM.Data.Seeding
{
    using System.Reflection;

    using EM.Common;
    using EM.Data.Models;
    using EM.Data.Seeding.Abstractions;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationRoleSeeder : ISeeder
    {
        private const string InvalidRoleStructureExceptionMessage = "Type {0} must contain only string constants.";

        private readonly RoleManager<ApplicationRole> roleManager;

        public ApplicationRoleSeeder(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            var roleNames = typeof(RoleConstants)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Select(x =>
                x.GetRawConstantValue() as string ??
                    throw new InvalidOperationException(string.Format(InvalidRoleStructureExceptionMessage, nameof(RoleConstants))))
                .ToArray();

            foreach (var roleName in roleNames)
            {
                bool roleExists = await this.roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
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
}
