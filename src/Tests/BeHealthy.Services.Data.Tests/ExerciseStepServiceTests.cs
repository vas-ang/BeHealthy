namespace BeHealthy.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using BeHealthy.Data;
    using BeHealthy.Data.Models;
    using BeHealthy.Data.Repositories;
    using BeHealthy.Services.Data.ExerciseSteps;
    using BeHealthy.Services.Data.Tests.Models.ExerciseStep;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ExerciseStepServiceTests
    {
        private const int Id = 1;
        private const string Heading = "Push up";
        private const string Url = "someUrl";
        private const string Description = "Just push up";
        private const string ChangedHeading = "New heading";
        private const string ChangedDescription = "New description";
        private const string NewImage = "New image";
        private readonly ApplicationDbContext db;
        private readonly EfDeletableEntityRepository<ExerciseStep> exerciseStepRepository;
        private readonly ExerciseStepService exerciseStepService;

        public ExerciseStepServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.db = new ApplicationDbContext(options.Options);
            this.exerciseStepRepository = new EfDeletableEntityRepository<ExerciseStep>(this.db);
            this.exerciseStepService = new ExerciseStepService(this.exerciseStepRepository);

            AutoMapperConfig.RegisterMappings(typeof(CreateExerciseStepTestModel).Assembly);
        }

        [Fact]
        public async Task CreateExerciseStepWorksCorrectly()
        {
            var model = new CreateExerciseStepTestModel
            {
                Heading = Heading,
                Description = Description,
            };

            var id = await this.exerciseStepService.CreateExerciseStepAsync<CreateExerciseStepTestModel>(model, Url);

            var actual = this.db.ExerciseSteps.Find(id);

            Assert.Equal(Heading, actual.Heading);
            Assert.Equal(Url, actual.Image);
            Assert.Equal(Description, actual.Description);
        }

        [Fact]
        public async Task DeleteExerciseStepWorksCorrectly()
        {
            await this.SeedAsync();

            await this.exerciseStepService.DeleteExerciseStepAsync(Id);

            var actual = await this.db.ExerciseSteps.CountAsync();

            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task ExerciseStepExistsReturnsCorrectly()
        {
            await this.SeedAsync();

            var expectedTrue = await this.exerciseStepService.ExerciseStepExistsAsync(Id);
            var expectedFalse = await this.exerciseStepService.ExerciseStepExistsAsync(69);

            Assert.True(expectedTrue);
            Assert.False(expectedFalse);
        }

        [Fact]
        public async Task GetExerciseStepReturnsCorrectly()
        {
            await this.SeedAsync();

            var actual = await this.exerciseStepService.GetExerciseStepAsync<GetExerciseStepTestModel>(Id);
            var invalid = await this.exerciseStepService.GetExerciseStepAsync<GetExerciseStepTestModel>(69);

            Assert.Equal(Heading, actual.Heading);
            Assert.Equal(Url, actual.Image);
            Assert.Equal(Description, actual.Description);
            Assert.Null(invalid);
        }

        [Fact]
        public async Task GetExerciseStepsCountReturnsCorrectly()
        {
            var expected = 5;

            await this.SeedManyAsync(expected, "pushup");

            var actual = await this.exerciseStepService.GetExerciseStepsCountAsync("pushup");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task UpdateExerciseStepReturnsCorrectly()
        {
            await this.SeedAsync();

            var model = new CreateExerciseStepTestModel
            {
                Id = Id,
                Heading = ChangedHeading,
                Description = ChangedDescription,
            };

            await this.exerciseStepService.UpdateExerciseStepAsync(model, string.Empty);

            var actual = this.db.ExerciseSteps.Find(Id);

            Assert.Equal(ChangedHeading, actual.Heading);
            Assert.Equal(Url, actual.Image);
            Assert.Equal(ChangedDescription, actual.Description);
        }

        [Fact]
        public async Task UpdateExerciseStepWithPictureReturnsCorrectly()
        {
            await this.SeedAsync();

            var model = new CreateExerciseStepTestModel
            {
                Id = Id,
                Heading = ChangedHeading,
                Description = ChangedDescription,
            };

            await this.exerciseStepService.UpdateExerciseStepAsync(model, NewImage);

            var actual = this.db.ExerciseSteps.Find(Id);

            Assert.Equal(ChangedHeading, actual.Heading);
            Assert.Equal(NewImage, actual.Image);
            Assert.Equal(ChangedDescription, actual.Description);
        }

        private async Task SeedAsync()
        {
            await this.db.ExerciseSteps.AddAsync(new ExerciseStep
            {
                Id = Id,
                Heading = Heading,
                Image = Url,
                Description = Description,
            });

            await this.db.SaveChangesAsync();

            await this.db.ExerciseSteps.ForEachAsync(x => this.db.Entry(x).State = EntityState.Detached);
        }

        private async Task SeedManyAsync(int count, string exerciseId)
        {
            var exercise = new Exercise
            {
                Id = exerciseId,
            };

            this.db.Exercises.Add(exercise);

            for (int i = 0; i < count; i++)
            {
                await this.db.ExerciseSteps.AddAsync(new ExerciseStep
                {
                    Exercise = exercise,
                });
            }

            await this.db.SaveChangesAsync();
        }
    }
}
