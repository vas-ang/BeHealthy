namespace BeHealthy.Services.Data.Tags
{
    using System.Threading.Tasks;

    public interface ITagService
    {
        public Task<T> CreateExerciseTagAsync<T>(string exerciseId, string name);

        public Task DeleteExerciseTagAsync<T>(T inputModel);

        public Task<bool> ExerciseTagExistsAsync(string exerciseId, string name);

        public Task<bool> ExerciseTagExistsAsync(string exerciseId, int tagId);
    }
}
