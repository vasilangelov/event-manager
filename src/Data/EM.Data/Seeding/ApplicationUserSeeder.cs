namespace EM.Data.Seeding
{
    using System.Threading.Tasks;

    using EM.Data.Models;
    using EM.Data.Seeding.Abstractions;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationUserSeeder : ISeeder
    {
        private const string UserNotCreatedExceptionMessage = "User {0} could not be created. Error list: {1}";

        private static readonly IEnumerable<UserInfo> usersInfo = new UserInfo[]
        {
            new UserInfo("admin", "admin@example.com", "admin123", "Admin" ),
            new UserInfo("user", "user@example.com", "user123"),
        };

        private record class UserInfo(string Username, string Email, string Password, params string[] Roles);

        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationUserSeeder(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task SeedAsync()
        {
            bool usersExist = await this.userManager.Users.AnyAsync();

            if (usersExist)
            {
                return;
            }

            foreach (var userInfo in usersInfo)
            {
                var user = await CreateUserAsync(userInfo);

                await this.userManager.AddToRolesAsync(user, userInfo.Roles);
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(UserInfo userInfo)
        {
            var user = new ApplicationUser();

            await this.userManager.SetUserNameAsync(user, userInfo.Username);
            await this.userManager.SetEmailAsync(user, userInfo.Email);

            var result = await this.userManager.CreateAsync(user, userInfo.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();

                throw new InvalidOperationException(string.Format(
                    UserNotCreatedExceptionMessage,
                    userInfo.Username,
                    string.Join(", ", errors)));
            }

            return user;
        }
    }
}
