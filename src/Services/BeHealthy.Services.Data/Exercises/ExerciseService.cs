namespace BeHealthy.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

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

        public T GetExercise<T>(string exerciseId)
            => this.exerciseRepository.All().Where(x => !x.IsDeleted).To<T>().FirstOrDefault();
    }
}
