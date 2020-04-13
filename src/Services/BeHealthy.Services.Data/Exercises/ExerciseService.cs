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
            => (await this.exerciseRepository.All().FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == exerciseId))?.CreatorId == userId;

        public async Task<T> GetExerciseAsync<T>(string exerciseId)
            => await this.exerciseRepository.All().Where(x => !x.IsDeleted && x.Id == exerciseId).To<T>().FirstOrDefaultAsync();
    }
}
