[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]

namespace BeHealthy.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data;
    using BeHealthy.Data.Models;
    using BeHealthy.Data.Repositories;
    using BeHealthy.Services.Data.Exercises;
    using BeHealthy.Services.Data.Tests.Models.Exercises;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ExercisesServiceTests
    {
        public const string PushUpName = "Push up";
        public const int PushUpStepId = 1;
        public const string PushUpDescription = "Very good chest exercise";
        public const string PushUpId = "pushup";
        public const bool PushUpIsPublished = false;
        public const string PushUpCreatorId = "pushupCreator";
        public const string PullUpName = "Pull up";
        public const string PullUpDescription = "Very good back exercise";
        public const string PullUpId = "pullup";
        public const bool PullUpIsPublished = true;
        public const string PullUpCreatorId = "pullupCreator";

        public const string Invalid = "invalid";

        private readonly ApplicationDbContext db;
        private readonly EfDeletableEntityRepository<Exercise> exerciseRepository;
        private readonly IExercisesService exerciseService;

        public ExercisesServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.db = new ApplicationDbContext(options.Options);
            this.exerciseRepository = new EfDeletableEntityRepository<Exercise>(this.db);
            this.exerciseService = new ExercisesService(this.exerciseRepository);

            AutoMapperConfig.RegisterMappings(typeof(CreateExerciseTestModel).Assembly);
        }

        [Fact]
        public async Task CreateExerciseCreatesAndReturnsExerciseId()
        {
            var model = new CreateExerciseTestModel { Name = PushUpName, Description = PushUpDescription };

            var result = await this.exerciseService.CreateAsync<CreateExerciseTestModel>(model, "test");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task IsUserExerciseCreatorReturnsCorrectly()
        {
            await this.SeedAsync();

            var expectedTrue = await this.exerciseService.IsUserExerciseCreatorAsync(PushUpId, PushUpCreatorId);
            var expectedFalseCreator = await this.exerciseService.IsUserExerciseCreatorAsync(PushUpId, Invalid);
            var expectedFalseExercise = await this.exerciseService.IsUserExerciseCreatorAsync(Invalid, PushUpCreatorId);

            Assert.True(expectedTrue);
            Assert.False(expectedFalseCreator);
            Assert.False(expectedFalseExercise);
        }

        [Fact]
        public async Task GetExerciseShouldReturnCorrectly()
        {
            await this.SeedAsync();

            var expectedNotNull = await this.exerciseService.GetExerciseAsync<GetExerciseTestModel>(PushUpId);
            var expectedNull = await this.exerciseService.GetExerciseAsync<GetExerciseTestModel>(Invalid);

            Assert.NotNull(expectedNotNull);
            Assert.Null(expectedNull);
        }

        [Fact]
        public async Task ChangePublishStateWorksCorrectly()
        {
            await this.SeedAsync();

            var expectedTrue = await this.exerciseService.ChangePublishState(PushUpId);
            var expectedFalse = await this.exerciseService.ChangePublishState(PushUpId);

            Assert.True(expectedTrue);
            Assert.False(expectedFalse);
        }

        [Fact]
        public async Task ExerciseExistsReturnsCorrectly()
        {
            await this.SeedAsync();

            var expectedTrue = await this.exerciseService.ExerciseExistsAsync(PushUpId);
            var expectedFalse = await this.exerciseService.ExerciseExistsAsync(Invalid);

            Assert.True(expectedTrue);
            Assert.False(expectedFalse);
        }

        [Fact]
        public async Task GetExerciseIdByStepIdReturnsCorrectly()
        {
            await this.SeedWithStepAsync();

            var actual = await this.exerciseService.GetExerciseIdByStepIdAsync(PushUpStepId);

            Assert.Equal(PushUpId, actual);
        }

        [Fact]
        public async Task GetPublishedExercisesReturnsOnlyPublished()
        {
            await this.SeedAsync();

            var expected = 1;

            var actual = (await this.exerciseService.GetPublishedExercisesAsync<GetExerciseTestModel, DateTime>(1, 5, x => x.CreatedOn)).Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetPublishedExercisesReturnsPerPage()
        {
            await this.SeedManyAsync(15, true);

            var expected = 5;

            var actual = (await this.exerciseService.GetPublishedExercisesAsync<GetExerciseTestModel, DateTime>(1, 5, x => x.CreatedOn)).Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetPublishedExercisesReturnsCorrectCount()
        {
            await this.SeedManyAsync(13, true);

            var expected = 3;

            var actual = (await this.exerciseService.GetPublishedExercisesAsync<GetExerciseTestModel, DateTime>(3, 5, x => x.CreatedOn)).Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetUnpublishedExercisesReturnsOnlyUnpublished()
        {
            await this.SeedAsync();

            var expected = 1;

            var actual = (await this.exerciseService.GetUnpublishedExercisesAsync<GetExerciseTestModel>(PushUpCreatorId)).Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task IsExercisePublishedReturnsCorrectly()
        {
            await this.SeedAsync();

            var expectedTrue = await this.exerciseService.IsExercisePublishedAsync(PullUpId);
            var expectedFalse = await this.exerciseService.IsExercisePublishedAsync(PushUpId);

            Assert.True(expectedTrue);
            Assert.False(expectedFalse);
        }

        [Fact]
        public async Task GetPublishedExercisesPagesCountReturnsCorrect()
        {
            await this.SeedManyAsync(10, true);
            await this.SeedManyAsync(5, false);

            var expected = 2;
            var actual = await this.exerciseService.GetPublishedExercisesPagesCountAsync(5);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetPublishedExercisesWithTagReturnPerPage()
        {
            await this.SeedManyWithTagAsync(10, "sport", true);

            var expected = 5;

            var result = (await this.exerciseService.GetPublishedExercisesWithTagAsync<GetExerciseTestModel, DateTime>(1, 5, "sport", x => x.CreatedOn)).Count();

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetPublishedExercisesWithTagLastPageReturnsCorrectCount()
        {
            await this.SeedManyWithTagAsync(13, "sport", true);

            var expected = 3;

            var actual = (await this.exerciseService.GetPublishedExercisesWithTagAsync<GetExerciseTestModel, DateTime>(3, 5, "sport", x => x.CreatedOn)).Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetPublishedExercisesWithTagPagesCountReturnsCorrectCount()
        {
            await this.SeedManyWithTagAsync(13, "sport", true);

            var expected = 3;

            var actual = await this.exerciseService.GetPublishedExercisesWithTagPagesCountAsync(5, "sport");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task DeleteExerciseDeletesExercise()
        {
            await this.SeedAsync();
            await this.exerciseService.DeleteExerciseAsync(PushUpId);

            var expected = 1;
            var actual = await this.exerciseRepository.AllAsNoTracking().CountAsync();

            Assert.Equal(expected, actual);
        }

        private async Task SeedAsync()
        {
            var pushup = new Exercise
            {
                Id = PushUpId,
                Name = PushUpName,
                IsPublished = PushUpIsPublished,
                Description = PushUpDescription,
                CreatorId = PushUpCreatorId,
            };

            await this.exerciseRepository.AddAsync(pushup);

            await this.exerciseRepository.AddAsync(new Exercise
            {
                Id = PullUpId,
                Name = PullUpName,
                IsPublished = PullUpIsPublished,
                Description = PullUpDescription,
                CreatorId = PullUpCreatorId,
            });

            await this.exerciseRepository.SaveChangesAsync();

            await this.exerciseRepository.AllWithDeleted().ForEachAsync(x => this.db.Entry(x).State = EntityState.Detached);
        }

        private async Task SeedWithStepAsync()
        {
            var pushup = new Exercise
            {
                Id = PushUpId,
                Name = PushUpName,
                IsPublished = PushUpIsPublished,
                Description = PushUpDescription,
                CreatorId = PushUpCreatorId,
            };

            await this.exerciseRepository.AddAsync(pushup);
            ExerciseStep step = new ExerciseStep
            {
                Id = PushUpStepId,
                Exercise = pushup,
                Heading = Guid.NewGuid().ToString(),
                Image = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
            };

            await this.db.ExerciseSteps.AddAsync(step);

            await this.db.SaveChangesAsync();
        }

        private async Task SeedManyAsync(int count, bool published = false)
        {
            for (int i = 0; i < count; i++)
            {
                await this.exerciseRepository.AddAsync(new Exercise
                {
                    CreatorId = "creator",
                    Name = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                    IsPublished = published,
                });
            }

            await this.exerciseRepository.SaveChangesAsync();
        }

        private async Task SeedManyWithTagAsync(int count, string tagName, bool published = false)
        {
            Tag tag = new Tag
            {
                Name = tagName,
            };

            for (int i = 0; i < count; i++)
            {
                await this.db.ExerciseTags.AddAsync(new ExerciseTag
                {
                    Exercise = new Exercise
                    {
                        Name = Guid.NewGuid().ToString(),
                        Description = Guid.NewGuid().ToString(),
                        IsPublished = published,
                    },
                    Tag = tag,
                });
            }

            await this.db.SaveChangesAsync();
        }
    }
}
