namespace BeHealthy.Services.Data.WorkoutExercises
{
    using System.Threading.Tasks;

    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class WorkoutExercisesService : IWorkoutExercisesService
    {
        private readonly IRepository<WorkoutExercise> workoutExercisesRepository;

        public WorkoutExercisesService(IRepository<WorkoutExercise> workoutExercisesRepository)
        {
            this.workoutExercisesRepository = workoutExercisesRepository;
        }

        public async Task AddWorkoutExerciseAsync<T>(T inputModel)
        {
            var workoutExercise = inputModel.To<WorkoutExercise>();

            await this.workoutExercisesRepository.AddAsync(workoutExercise);

            await this.workoutExercisesRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteWorkoutExerciseAsync(string workoutId, string exerciseId)
        {
            var workoutExercise = await this.workoutExercisesRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.WorkoutId == workoutId && x.ExerciseId == exerciseId);

            if (workoutExercise == null)
            {
                return false;
            }

            this.workoutExercisesRepository.Delete(workoutExercise);

            await this.workoutExercisesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> WorkoutExerciseExistsAsync(string workoutId, string exerciseId)
            => await this.workoutExercisesRepository.AllAsNoTracking().AnyAsync(x => x.WorkoutId == workoutId && x.ExerciseId == exerciseId);
    }
}
