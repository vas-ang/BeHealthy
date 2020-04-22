namespace BeHealthy.Services.Data.Reviews
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ReviewService : IReviewService
    {
        private readonly IRepository<ExerciseReview> exerciseReviewsRepository;

        public ReviewService(IRepository<ExerciseReview> exerciseReviewsRepository)
        {
            this.exerciseReviewsRepository = exerciseReviewsRepository;
        }

        public async Task<TOutput> CreateExerciseReviewAsync<TInput, TOutput>(TInput inputModel, string userId)
        {
            var exerciseReview = inputModel.To<ExerciseReview>();
            exerciseReview.AuthorId = userId;

            await this.exerciseReviewsRepository.AddAsync(exerciseReview);

            await this.exerciseReviewsRepository.SaveChangesAsync();

            return await this.exerciseReviewsRepository
                .AllAsNoTracking()
                .Where(x => x.ExerciseId == exerciseReview.ExerciseId && x.AuthorId == userId)
                .To<TOutput>()
                .FirstOrDefaultAsync();
        }

        public async Task DeleteExerciseReviewAsync(string exerciseId, string userId)
        {
            var exerciseReview = await this.exerciseReviewsRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.ExerciseId == exerciseId && x.AuthorId == userId);

            this.exerciseReviewsRepository.Delete(exerciseReview);

            await this.exerciseReviewsRepository.SaveChangesAsync();
        }

        public async Task<TOutput> EditExerciseReviewAsync<TInput, TOutput>(TInput inputModel, string userId)
        {
            var exerciseReview = inputModel.To<ExerciseReview>();
            exerciseReview.AuthorId = userId;

            this.exerciseReviewsRepository.Update(exerciseReview);

            await this.exerciseReviewsRepository.SaveChangesAsync();

            return await this.exerciseReviewsRepository
               .AllAsNoTracking()
               .Where(x => x.ExerciseId == exerciseReview.ExerciseId && x.AuthorId == userId)
               .To<TOutput>()
               .FirstOrDefaultAsync();
        }

        public async Task<bool> ExerciseReviewExistsAsync(string exerciseId, string userId)
            => await this.exerciseReviewsRepository.AllAsNoTracking().AnyAsync(x => x.ExerciseId == exerciseId && x.AuthorId == userId);

        public async Task<int> GetExerciseReviewRatingAsync(string exerciseId, string userId)
            => await this.exerciseReviewsRepository
                .AllAsNoTracking()
                .Where(x => x.ExerciseId == exerciseId && x.AuthorId == userId)
                .Select(x => x.Rating)
                .FirstOrDefaultAsync();
    }
}
