namespace BeHealthy.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using BeHealthy.Data;
    using BeHealthy.Data.Models;
    using BeHealthy.Data.Repositories;
    using BeHealthy.Services.Data.Tests.Models.WorkoutExercise;
    using BeHealthy.Services.Data.WorkoutExercises;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class WorkoutExerciseServiceTests
    {
        private readonly ApplicationDbContext db;
        private readonly EfRepository<WorkoutExercise> workoutExerciseRepository;
        private readonly WorkoutExerciseService workoutExerciseService;

        public WorkoutExerciseServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.db = new ApplicationDbContext(options.Options);
            this.workoutExerciseRepository = new EfRepository<WorkoutExercise>(this.db);
            this.workoutExerciseService = new WorkoutExerciseService(this.workoutExerciseRepository);

            AutoMapperConfig.RegisterMappings(typeof(WorkoutExerciseInputTestModel).Assembly);
        }

        [Fact]
        public async Task CreateWorkoutExerciseWorksCorrectly()
        {
            var input = new WorkoutExerciseInputTestModel
            {
                ExerciseId = "exercise",
                WorkoutId = "workout",
                Sets = 4,
                Repetitions = 12,
            };

            await this.workoutExerciseService.AddWorkoutExerciseAsync(input);

            var actual = await this.db.WorkoutExercises.CountAsync();

            Assert.Equal(1, actual);
        }

        [Theory]
        [InlineData("workout", "exercise", true)]
        [InlineData("workout", "invalid", false)]
        [InlineData("invalid", "exercise", false)]
        [InlineData("invalid", "invalid", false)]
        public async Task DeleteWorkoutExerciseWorksCorrectly(string workoutId, string exerciseId, bool expected)
        {
            await this.SeedAsync();

            var actual = await this.workoutExerciseService.DeleteWorkoutExerciseAsync(workoutId, exerciseId);

            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("workout", "exercise", true)]
        [InlineData("workout", "invalid", false)]
        [InlineData("invalid", "exercise", false)]
        [InlineData("invalid", "invalid", false)]
        public async Task WorkoutExerciseExistsWorksCorrectly(string workoutId, string exerciseId, bool expected)
        {
            await this.SeedAsync();

            var actual = await this.workoutExerciseService.WorkoutExerciseExistsAsync(workoutId, exerciseId);

            Assert.Equal(actual, expected);
        }

        private async Task SeedAsync()
        {
            var workoutExercise = new WorkoutExercise
            {
                ExerciseId = "exercise",
                WorkoutId = "workout",
                Sets = 4,
                Repetitions = 12,
            };

            await this.db.WorkoutExercises.AddAsync(workoutExercise);

            await this.db.SaveChangesAsync();

            this.db.Entry(workoutExercise).State = EntityState.Detached;
        }
    }
}
