namespace BeHealthy.Services.Data
{
    using System.Threading.Tasks;

    public interface IExerciseService
    {
        public Task<string> CreateAsync<T>(T exerciseInputModel, string creatorId);

        public T GetExercise<T>(string exerciseId);
    }
}
