namespace BeHealthy.Services.Data.ExerciseSteps
{
    using System.Threading.Tasks;

    public interface IExerciseStepService
    {
        public Task<int> CreateExerciseStepAsync<T>(T exerciseStepInputModel, string imgeUrl);

        public Task<int> GetExerciseStepsCountAsync(string exerciseId);

        public Task<bool> ExerciseStepExistsAsync(int exerciseStepId);

        public Task DeleteExerciseStepAsync(int exerciseStepId);

        public Task<T> GetExerciseStepAsync<T>(int exerciseStepId);

        public Task UpdateExerciseStepAsync<T>(T inputModel, string imageUrl);
    }
}
