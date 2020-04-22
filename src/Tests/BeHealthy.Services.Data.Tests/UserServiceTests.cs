namespace BeHealthy.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using BeHealthy.Data;
    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Data.Repositories;
    using BeHealthy.Services.Data.Tests.Models.User;
    using BeHealthy.Services.Data.Users;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class UserServiceTests
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly UserService userService;
        private readonly ApplicationDbContext db;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.db = new ApplicationDbContext(options.Options);
            this.usersRepository = new EfDeletableEntityRepository<ApplicationUser>(this.db);
            this.userService = new UserService(this.usersRepository);

            AutoMapperConfig.RegisterMappings(typeof(UserOutputTestModel).Assembly);
        }

        [Fact]
        public async Task GetUserDetailsWorksCorrectly()
        {
            await this.SeedAsync();

            var model = await this.userService.GetUserDetailsAsync<UserOutputTestModel>("pesho");

            Assert.Equal("pesho", model.UserName);
            Assert.Equal("pesho@softuni.bg", model.Email);
        }

        [Theory]
        [InlineData("pesho", true)]
        [InlineData("invalid", false)]
        public async Task DeleteWorkoutExerciseWorksCorrectly(string username, bool expected)
        {
            await this.SeedAsync();

            var actual = await this.userService.UserExistsAsync(username);

            Assert.Equal(actual, expected);
        }

        private async Task SeedAsync()
        {
            var user = new ApplicationUser
            {
                UserName = "pesho",
                Email = "pesho@softuni.bg",
            };

            await this.db.Users.AddAsync(user);

            await this.db.SaveChangesAsync();

            this.db.Entry(user).State = EntityState.Detached;
        }
    }
}
