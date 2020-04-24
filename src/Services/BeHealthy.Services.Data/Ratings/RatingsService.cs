namespace BeHealthy.Services.Data.Ratings
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class RatingsService : IRatingsService
    {
        private readonly IRepository<ExerciseRating> exerciseReviewsRepository;

        public RatingsService(IRepository<ExerciseRating> exerciseReviewsRepository)
        {
            this.exerciseReviewsRepository = exerciseReviewsRepository;
        }

        public async Task<TOutput> CreateExerciseReviewAsync<TInput, TOutput>(TInput inputModel, string userId)
        {
            var exerciseReview = inputModel.To<ExerciseRating>();
            exerciseReview.UserId = userId;

            await this.exerciseReviewsRepository.AddAsync(exerciseReview);

            await this.exerciseReviewsRepository.SaveChangesAsync();

            return await this.exerciseReviewsRepository
                .AllAsNoTracking()
                .Where(x => x.ExerciseId == exerciseReview.ExerciseId && x.UserId == userId)
                .To<TOutput>()
                .FirstOrDefaultAsync();
        }

        public async Task DeleteExerciseReviewAsync(string exerciseId, string userId)
        {
            var exerciseReview = await this.exerciseReviewsRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.ExerciseId == exerciseId && x.UserId == userId);

            this.exerciseReviewsRepository.Delete(exerciseReview);

            await this.exerciseReviewsRepository.SaveChangesAsync();
        }

        public async Task<TOutput> EditExerciseReviewAsync<TInput, TOutput>(TInput inputModel, string userId)
        {
            var exerciseReview = inputModel.To<ExerciseRating>();
            exerciseReview.UserId = userId;

            this.exerciseReviewsRepository.Update(exerciseReview);

            await this.exerciseReviewsRepository.SaveChangesAsync();

            return await this.exerciseReviewsRepository
               .AllAsNoTracking()
               .Where(x => x.ExerciseId == exerciseReview.ExerciseId && x.UserId == userId)
               .To<TOutput>()
               .FirstOrDefaultAsync();
        }

        public async Task<bool> ExerciseReviewExistsAsync(string exerciseId, string userId)
            => await this.exerciseReviewsRepository.AllAsNoTracking().AnyAsync(x => x.ExerciseId == exerciseId && x.UserId == userId);

        public async Task<int> GetExerciseReviewRatingAsync(string exerciseId, string userId)
            => await this.exerciseReviewsRepository
                .AllAsNoTracking()
                .Where(x => x.ExerciseId == exerciseId && x.UserId == userId)
                .Select(x => x.Rating)
                .FirstOrDefaultAsync();
    }
}
