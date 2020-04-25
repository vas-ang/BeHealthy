namespace BeHealthy.Services.Data.Workouts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWorkoutsService
    {
        public Task<string> CreateAsync<T>(T workoutInputModel, string creatorId);

        public Task EditAsync<T>(T editInputModel);

        public Task<bool> IsUserWorkoutCreatorAsync(string workoutId, string userId);

        public Task DeleteWorkoutAsync(string workoutId);

        public Task<IEnumerable<T>> GetAllUserWorkoutsAsync<T>(string userId);

        public Task<T> GetWorkoutAsync<T>(string workoutId);
    }
}
