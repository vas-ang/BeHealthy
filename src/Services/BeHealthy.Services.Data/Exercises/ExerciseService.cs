namespace BeHealthy.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ExerciseService : IExerciseService
    {
        private readonly IDeletableEntityRepository<Exercise> exerciseRepository;

        public ExerciseService(IDeletableEntityRepository<Exercise> exerciseRepository)
        {
            this.exerciseRepository = exerciseRepository;
        }

        public async Task<string> CreateAsync<T>(T exerciseInputModel, string creatorId)
        {
            var exercise = exerciseInputModel.To<Exercise>();

            exercise.CreatorId = creatorId;

            await this.exerciseRepository.AddAsync(exercise);

            await this.exerciseRepository.SaveChangesAsync();

            return exercise.Id;
        }

        public async Task<bool> IsUserExerciseCreatorAsync(string exerciseId, string userId)
            => (await this.exerciseRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == exerciseId))?.CreatorId == userId;

        public async Task<T> GetExerciseAsync<T>(string exerciseId)
            => await this.exerciseRepository.AllAsNoTracking().Where(x => x.Id == exerciseId).To<T>().FirstOrDefaultAsync();

        public async Task<bool> ChangePublishState(string exerciseId)
        {
            var exercise = await this.exerciseRepository.All().FirstOrDefaultAsync(x => x.Id == exerciseId);

            exercise.IsPublished = !exercise.IsPublished;

            this.exerciseRepository.Update(exercise);

            await this.exerciseRepository.SaveChangesAsync();

            return exercise.IsPublished;
        }

        public async Task<bool> ExerciseExistsAsync(string exerciseId)
            => (await this.exerciseRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == exerciseId)) != null;

        public async Task<string> GetExerciseIdByStepIdAsync(int exerciseStepId)
            => (await this.exerciseRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.ExerciseSteps.Any(es => es.Id == exerciseStepId)))?.Id;
    }
}
