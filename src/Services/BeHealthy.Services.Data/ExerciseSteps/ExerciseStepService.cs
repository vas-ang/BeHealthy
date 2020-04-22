namespace BeHealthy.Services.Data.ExerciseSteps
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ExerciseStepService : IExerciseStepService
    {
        private readonly IDeletableEntityRepository<ExerciseStep> exerciseStepRepository;

        public ExerciseStepService(IDeletableEntityRepository<ExerciseStep> exerciseStepRepository)
        {
            this.exerciseStepRepository = exerciseStepRepository;
        }

        public async Task<int> CreateExerciseStepAsync<T>(T exerciseStepInputModel, string imageUrl)
        {
            var exerciseStep = exerciseStepInputModel.To<ExerciseStep>();

            exerciseStep.Image = imageUrl;

            await this.exerciseStepRepository.AddAsync(exerciseStep);

            await this.exerciseStepRepository.SaveChangesAsync();

            return exerciseStep.Id;
        }

        public async Task DeleteExerciseStepAsync(int exerciseStepId)
        {
            var exerciseStep = await this.exerciseStepRepository.All().FirstOrDefaultAsync(x => x.Id == exerciseStepId);

            this.exerciseStepRepository.Delete(exerciseStep);

            await this.exerciseStepRepository.SaveChangesAsync();
        }

        public async Task<bool> ExerciseStepExistsAsync(int exerciseStepId)
            => await this.exerciseStepRepository.AllAsNoTracking().AnyAsync(x => x.Id == exerciseStepId);

        public async Task<T> GetExerciseStepAsync<T>(int exerciseStepId)
            => await this.exerciseStepRepository.AllAsNoTracking().Where(x => x.Id == exerciseStepId).To<T>().FirstOrDefaultAsync();

        public async Task<int> GetExerciseStepsCountAsync(string exerciseId)
            => await this.exerciseStepRepository.AllAsNoTracking().Where(x => x.ExerciseId == exerciseId).CountAsync();

        public async Task UpdateExerciseStepAsync<T>(T inputModel, string imageUrl)
        {
            var updatedExerciseStep = inputModel.To<ExerciseStep>();

            var exerciseStep = await this.exerciseStepRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == updatedExerciseStep.Id);

            exerciseStep.Heading = updatedExerciseStep.Heading;
            exerciseStep.Description = updatedExerciseStep.Description;

            if (imageUrl != string.Empty)
            {
                exerciseStep.Image = imageUrl;
            }

            this.exerciseStepRepository.Update(exerciseStep);

            await this.exerciseStepRepository.SaveChangesAsync();
        }
    }
}
