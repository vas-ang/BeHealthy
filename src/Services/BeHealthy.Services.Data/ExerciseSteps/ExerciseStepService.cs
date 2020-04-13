namespace BeHealthy.Services.Data.ExerciseSteps
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

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

        public int GetExerciseStepsCount(string exerciseId)
            => this.exerciseStepRepository.All().Where(x => x.ExerciseId == exerciseId).Count();
    }
}
