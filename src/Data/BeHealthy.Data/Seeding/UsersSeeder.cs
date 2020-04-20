namespace BeHealthy.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Common;
    using BeHealthy.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class UsersSeeder : ISeeder
    {
        public const string Password = "123456";

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var user = new ApplicationUser
            {
                UserName = "angeloff",
                Email = "angeloff@softuni.bg",
                EmailConfirmed = true,
            };

            var spectator = new ApplicationUser
            {
                UserName = "angeloff.spectator",
                Email = "angeloff.spectator@softuni.bg",
                EmailConfirmed = true,
            };

            var admin = new ApplicationUser
            {
                UserName = "angeloff.admin",
                Email = "angeloff.admin@softuni.bg",
                EmailConfirmed = true,
            };

            await userManager.CreateAsync(user, Password);

            await userManager.AddToRoleAsync(user, GlobalConstants.UserRoleName);

            await userManager.CreateAsync(spectator, Password);

            await userManager.AddToRoleAsync(spectator, GlobalConstants.UserRoleName);

            await userManager.CreateAsync(admin, Password);

            await userManager.AddToRoleAsync(admin, GlobalConstants.AdministratorRoleName);
        }
    }
}
