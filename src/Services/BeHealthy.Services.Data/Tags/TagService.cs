namespace BeHealthy.Services.Data.Tags
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class TagService : ITagService
    {
        private readonly IDeletableEntityRepository<Tag> tagsRepository;
        private readonly IRepository<ExerciseTag> exerciseTagsRepository;

        public TagService(IDeletableEntityRepository<Tag> tagsRepository, IRepository<ExerciseTag> exerciseTagsRepository)
        {
            this.tagsRepository = tagsRepository;
            this.exerciseTagsRepository = exerciseTagsRepository;
        }

        public async Task<T> CreateExerciseTagAsync<T>(string exerciseId, string name)
        {
            name = name.Replace(' ', '-').Trim().ToLower();

            var tag = await this.tagsRepository.All()
                .Where(x => x.Name == name && x.ExerciseTags.All(y => y.ExerciseId != exerciseId))
                .FirstOrDefaultAsync();

            if (tag == null)
            {
                tag = new Tag
                {
                    Name = name,
                };

                await this.tagsRepository.AddAsync(tag);

                await this.tagsRepository.SaveChangesAsync();
            }

            var exerciseTag = new ExerciseTag
            {
                ExerciseId = exerciseId,
                Tag = tag,
            };

            await this.exerciseTagsRepository.AddAsync(exerciseTag);

            await this.exerciseTagsRepository.SaveChangesAsync();

            return tag.To<T>();
        }

        public async Task DeleteExerciseTagAsync<T>(T inputModel)
        {
            var exerciseTag = inputModel.To<ExerciseTag>();

            this.exerciseTagsRepository.Delete(exerciseTag);

            await this.exerciseTagsRepository.SaveChangesAsync();
        }

        public async Task<bool> ExerciseTagExistsAsync(string exerciseId, string name)
            => await this.exerciseTagsRepository.AllAsNoTracking().AnyAsync(x => x.ExerciseId == exerciseId && x.Tag.Name == name);

        public async Task<bool> ExerciseTagExistsAsync(string exerciseId, int tagId)
            => await this.exerciseTagsRepository.AllAsNoTracking().AnyAsync(x => x.ExerciseId == exerciseId && x.TagId == tagId);
    }
}
