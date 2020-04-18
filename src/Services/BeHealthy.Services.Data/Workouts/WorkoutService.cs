namespace BeHealthy.Services.Data.Workouts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class WorkoutService : IWorkoutService
    {
        private readonly IDeletableEntityRepository<Workout> workoutsRepository;

        public WorkoutService(IDeletableEntityRepository<Workout> workoutsRepository)
        {
            this.workoutsRepository = workoutsRepository;
        }

        public async Task<string> CreateAsync<T>(T workoutInputModel, string creatorId)
        {
            var workout = workoutInputModel.To<Workout>();

            workout.CreatorId = creatorId;

            await this.workoutsRepository.AddAsync(workout);

            await this.workoutsRepository.SaveChangesAsync();

            return workout.Id;
        }

        public async Task DeleteWorkoutAsync(string workoutId)
        {
            var workout = await this.workoutsRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == workoutId);

            this.workoutsRepository.Delete(workout);

            await this.workoutsRepository.SaveChangesAsync();
        }

        public async Task EditAsync<T>(T editInputModel)
        {
            var editedWorkout = editInputModel.To<Workout>();

            var workout = await this.workoutsRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == editedWorkout.Id);

            workout.Name = editedWorkout.Name;
            workout.Weekday = editedWorkout.Weekday;

            this.workoutsRepository.Update(workout);

            await this.workoutsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllUserWorkoutsAsync<T>(string userId)
            => await this.workoutsRepository
            .AllAsNoTracking()
            .Where(x => x.CreatorId == userId)
            .To<T>()
            .ToArrayAsync();

        public async Task<T> GetWorkoutAsync<T>(string workoutId)
            => await this.workoutsRepository
            .AllAsNoTracking()
            .Where(x => x.Id == workoutId)
            .To<T>()
            .FirstOrDefaultAsync();

        public async Task<bool> IsUserWorkoutCreatorAsync(string workoutId, string userId)
            => await this.workoutsRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == workoutId && x.CreatorId == userId) != null;

        public async Task<bool> WorkoutExistsAsync(string workoutId)
            => await this.workoutsRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == workoutId) != null;
    }
}
