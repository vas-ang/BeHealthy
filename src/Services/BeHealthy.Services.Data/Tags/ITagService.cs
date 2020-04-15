namespace BeHealthy.Services.Data.Tags
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITagService
    {
        public Task<IEnumerable<T>> GetExerciseTagsAsync<T>(string exeriseId);

        public Task<T> CreateExerciseTagAsync<T>(string exerciseId, string name);

        public Task<bool> ExerciseTagExistsAsync(string exerciseId, string name);
    }
}
