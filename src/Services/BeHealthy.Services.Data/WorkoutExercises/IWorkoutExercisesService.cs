namespace BeHealthy.Services.Data.WorkoutExercises
{
    using System.Threading.Tasks;

    public interface IWorkoutExercisesService
    {
        public Task AddWorkoutExerciseAsync<T>(T inputModel);

        public Task<bool> DeleteWorkoutExerciseAsync(string workoutId, string exerciseId);

        public Task<bool> WorkoutExerciseExistsAsync(string workoutId, string exerciseId);
    }
}
