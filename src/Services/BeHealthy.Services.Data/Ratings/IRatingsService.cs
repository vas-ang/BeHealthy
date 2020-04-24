namespace BeHealthy.Services.Data.Ratings
{
    using System.Threading.Tasks;

    public interface IRatingsService
    {
        public Task<TOutput> CreateExerciseReviewAsync<TInput, TOutput>(TInput inputModel, string userId);

        public Task<TOutput> EditExerciseReviewAsync<TInput, TOutput>(TInput inputModel, string userId);

        public Task DeleteExerciseReviewAsync(string exerciseId, string userId);

        public Task<int> GetExerciseReviewRatingAsync(string exerciseId, string userId);

        public Task<bool> ExerciseReviewExistsAsync(string exerciseId, string userId);
    }
}
