namespace BeHealthy.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data;
    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Data.Models.Enumerators;
    using BeHealthy.Data.Repositories;
    using BeHealthy.Services.Data.Tests.Models.Workouts;
    using BeHealthy.Services.Data.Workouts;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class WorkoutsServiceTests
    {
        private const string WorkoutName = "workout";
        private const string WorkoutCreatorId = "creator";
        private const string WorkoutId = "id";
        private readonly ApplicationDbContext db;
        private readonly IDeletableEntityRepository<Workout> workoutRepository;
        private readonly IWorkoutsService workoutService;

        public WorkoutsServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.db = new ApplicationDbContext(options.Options);
            this.workoutRepository = new EfDeletableEntityRepository<Workout>(this.db);
            this.workoutService = new WorkoutsService(this.workoutRepository);

            AutoMapperConfig.RegisterMappings(typeof(WorkoutInputTestModel).Assembly);
        }

        [Fact]
        public async Task CreateWorksCorrectly()
        {
            var input = new WorkoutInputTestModel
            {
                Id = WorkoutId,
                Name = WorkoutName,
                Weekday = Weekday.Monday,
            };

            var id = await this.workoutService.CreateAsync(input, WorkoutCreatorId);

            var output = this.db.Workouts.Find(id);

            Assert.Equal(WorkoutName, output.Name);
            Assert.Equal(Weekday.Monday, output.Weekday);
        }

        [Fact]
        public async Task DeleteWorkoutWorksCorrectly()
        {
            await this.SeedAsync();

            var expected = await this.db.Workouts.CountAsync() - 1;

            await this.workoutService.DeleteWorkoutAsync(WorkoutId);

            var actual = await this.db.Workouts.CountAsync();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task EditWorkoutWorksCorrectly()
        {
            await this.SeedAsync();

            var input = new WorkoutInputTestModel
            {
                Id = WorkoutId,
                Name = "editedWorkout",
                Weekday = Weekday.Tuesday,
            };

            await this.workoutService.EditAsync(input);

            var actual = this.db.Workouts.Find(WorkoutId);

            Assert.Equal("editedWorkout", actual.Name);
            Assert.Equal(Weekday.Tuesday, actual.Weekday);
        }

        [Fact]
        public async Task GetAllUserWorkoutsWorksCorrectly()
        {
            await this.SeedAsync();

            var actual = (await this.workoutService.GetAllUserWorkoutsAsync<WorkoutOutputTestModel>("userId")).Count();

            Assert.Equal(1, actual);
        }

        [Fact]
        public async Task GeWorkoutWorksCorrectly()
        {
            await this.SeedAsync();

            var actual = await this.workoutService.GetWorkoutAsync<WorkoutOutputTestModel>(WorkoutId);

            Assert.Equal(WorkoutName, actual.Name);
            Assert.Equal(Weekday.Monday, actual.Weekday);
        }

        [Theory]
        [InlineData(WorkoutId, "userId", true)]
        [InlineData("invalid", "userId", false)]
        [InlineData(WorkoutId, "invalid", false)]
        [InlineData("invalid", "invalid", false)]
        public async Task IsUserWorkoutCreatorWorksCorrectly(string workoutId, string userId, bool expected)
        {
            await this.SeedAsync();

            var actual = await this.workoutService.IsUserWorkoutCreatorAsync(workoutId, userId);

            Assert.Equal(expected, actual);
        }

        private async Task SeedAsync()
        {
            var workout = new Workout
            {
                Id = WorkoutId,
                Name = WorkoutName,
                Weekday = Weekday.Monday,
                CreatorId = "userId",
            };

            await this.db.Workouts.AddAsync(workout);

            await this.db.Workouts.AddAsync(new Workout
            {
                Id = WorkoutId + "2",
                Name = WorkoutName,
                Weekday = Weekday.Monday,
                CreatorId = "userId2",
            });

            await this.db.SaveChangesAsync();

            await this.db.Workouts.ForEachAsync(x => this.db.Entry(x).State = EntityState.Detached);
        }
    }
}
