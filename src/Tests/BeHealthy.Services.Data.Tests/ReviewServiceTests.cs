namespace BeHealthy.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using BeHealthy.Data;
    using BeHealthy.Data.Models;
    using BeHealthy.Data.Repositories;
    using BeHealthy.Services.Data.Reviews;
    using BeHealthy.Services.Data.Tests.Models.Review;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ReviewServiceTests
    {
        private const string Invalid = "invalid";
        private const string ExerciseId = "exercise";
        private const string AuthorId = "author";
        private readonly ApplicationDbContext db;
        private readonly EfRepository<ExerciseReview> exerciseReviewRepository;
        private readonly ReviewService reviewService;

        public ReviewServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.db = new ApplicationDbContext(options.Options);
            this.exerciseReviewRepository = new EfRepository<ExerciseReview>(this.db);
            this.reviewService = new ReviewService(this.exerciseReviewRepository);

            AutoMapperConfig.RegisterMappings(typeof(ReviewInputTestModel).Assembly);
        }

        [Fact]
        public async Task CreateExerciseReviewWorks()
        {
            var inputModel = new ReviewInputTestModel
            {
                Rating = 3,
                ExerciseId = ExerciseId,
            };

            var actual = await this.reviewService.CreateExerciseReviewAsync<ReviewInputTestModel, ReviewOutputTestModel>(inputModel, AuthorId);

            Assert.Equal(3, actual.Rating);
            Assert.Equal(ExerciseId, actual.ExerciseId);
        }

        [Fact]
        public async Task DeleteExerciseReviewWorksCorrectly()
        {
            await this.SeedAsync();

            var expected = await this.db.ExerciseReviews.CountAsync() - 1;

            await this.reviewService.DeleteExerciseReviewAsync(ExerciseId, AuthorId);

            var actual = await this.db.ExerciseReviews.CountAsync();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task EditExerciseReviewWorksCorrectly()
        {
            await this.SeedAsync();

            var inputModel = new ReviewInputTestModel
            {
                Rating = 3,
                ExerciseId = ExerciseId,
            };

            var actual = await this.reviewService.EditExerciseReviewAsync<ReviewInputTestModel, ReviewOutputTestModel>(inputModel, AuthorId);

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
            await this.db.ExerciseReviews.AddAsync(new ExerciseReview
            {
                AuthorId = AuthorId,
                ExerciseId = ExerciseId,
                Rating = 5,
            });

            await this.db.SaveChangesAsync();

            await this.db.ExerciseReviews.ForEachAsync(x => this.db.Entry(x).State = EntityState.Detached);
        }
    }
}
