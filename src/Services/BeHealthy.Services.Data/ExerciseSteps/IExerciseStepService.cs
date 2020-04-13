namespace BeHealthy.Services.Data.ExerciseSteps
{
    using System.Threading.Tasks;

    public interface IExerciseStepService
    {
        public Task<int> CreateExerciseStepAsync<T>(T exerciseStepInputModel, string imgeUrl);

        public int GetExerciseStepsCount(string exerciseId);
    }
}
