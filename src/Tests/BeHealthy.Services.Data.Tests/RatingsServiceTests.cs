namespace BeHealthy.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using BeHealthy.Data;
    using BeHealthy.Data.Models;
    using BeHealthy.Data.Repositories;
    using BeHealthy.Services.Data.Ratings;
    using BeHealthy.Services.Data.Tests.Models.Ratings;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class RatingsServiceTests
    {
        private const string Invalid = "invalid";
        private const string ExerciseId = "exercise";
        private const string AuthorId = "author";
        private readonly ApplicationDbContext db;
        private readonly RatingsService reviewService;

        public RatingsServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.db = new ApplicationDbContext(options.Options);
            var exerciseRatingsRepository = new EfRepository<ExerciseRating>(this.db);
            this.reviewService = new RatingsService(exerciseRatingsRepository);

            AutoMapperConfig.RegisterMappings(typeof(RatingInputTestModel).Assembly);
        }

        [Fact]
        public async Task CreateExerciseReviewWorks()
        {
            var inputModel = new RatingInputTestModel
            {
                Rating = 3,
                ExerciseId = ExerciseId,
            };

            var actual = await this.reviewService.CreateExerciseReviewAsync<RatingInputTestModel, RatingOutputTestModel>(inputModel, AuthorId);

            Assert.Equal(3, actual.Rating);
            Assert.Equal(ExerciseId, actual.ExerciseId);
        }

        [Fact]
        public async Task DeleteExerciseReviewWorksCorrectly()
        {
            await this.SeedAsync();

            var expected = await this.db.ExerciseRatings.CountAsync() - 1;

            await this.reviewService.DeleteExerciseReviewAsync(ExerciseId, AuthorId);

            var actual = await this.db.ExerciseRatings.CountAsync();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task EditExerciseReviewWorksCorrectly()
        {
            await this.SeedAsync();

            var inputModel = new RatingInputTestModel
            {
                Rating = 3,
                ExerciseId = ExerciseId,
            };

            var actual = await this.reviewService.EditExerciseReviewAsync<RatingInputTestModel, RatingOutputTestModel>(inputModel, AuthorId);

            Assert.Equal(inputModel.Rating, actual.Rating);
        }

        [Theory]
        [InlineData(ExerciseId, AuthorId, true)]
        [InlineData(Invalid, AuthorId, false)]
        [InlineData(ExerciseId, Invalid, false)]
        [InlineData(Invalid, Invalid, false)]
        public async Task ExerciseReviewExistsWorksCorrectly(string exerciseId, string authorId, bool expected)
        {
            await this.SeedAsync();

            var actual = await this.reviewService.ExerciseReviewExistsAsync(exerciseId, authorId);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetExerciseReviewRatingWorksCorrectly()
        {
            await this.SeedAsync();

            var actual = await this.reviewService.GetExerciseReviewRatingAsync(ExerciseId, AuthorId);

            Assert.Equal(5, actual);
        }

        private async Task SeedAsync()
        {
            await this.db.ExerciseRatings.AddAsync(new ExerciseRating
            {
                UserId = AuthorId,
                ExerciseId = ExerciseId,
                Rating = 5,
            });

            await this.db.SaveChangesAsync();

            await this.db.ExerciseRatings.ForEachAsync(x => this.db.Entry(x).State = EntityState.Detached);
        }
    }
}
