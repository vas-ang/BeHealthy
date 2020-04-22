namespace BeHealthy.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using BeHealthy.Data;
    using BeHealthy.Data.Models;
    using BeHealthy.Data.Repositories;
    using BeHealthy.Services.Data.Tags;
    using BeHealthy.Services.Data.Tests.Models.Tag;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class TagServiceTests
    {
        private const string ExerciseId = "exercise";
        private const string Invalid = "invalid";
        private const string TagName = "tag";
        private readonly ApplicationDbContext db;
        private readonly EfDeletableEntityRepository<Tag> tagRepository;
        private readonly EfRepository<ExerciseTag> exerciseTagRepository;
        private readonly TagService tagService;

        public TagServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.db = new ApplicationDbContext(options.Options);
            this.tagRepository = new EfDeletableEntityRepository<Tag>(this.db);
            this.exerciseTagRepository = new EfRepository<ExerciseTag>(this.db);
            this.tagService = new TagService(this.tagRepository, this.exerciseTagRepository);

            AutoMapperConfig.RegisterMappings(typeof(ExerciseTagInputTestModel).Assembly);
        }

        [Theory]
        [InlineData(ExerciseId, "TaG", TagName)]
        [InlineData(ExerciseId, "TaG g", "tag-g")]
        public async Task CreateExerciseTagWorksCorrectly(string exerciseId, string input, string expected)
        {
            var actual = await this.tagService.CreateExerciseTagAsync<TagOutputTestModel>(exerciseId, input);

            Assert.Equal(expected, actual.Name);
        }

        [Fact]
        public async Task DeleteExerciseTagWorksCorrectly()
        {
            await this.SeedAsync();

            var input = new ExerciseTagInputTestModel
            {
                ExerciseId = ExerciseId,
                TagId = 1,
            };

            var expected = await this.db.ExerciseTags.CountAsync() - 1;

            await this.tagService.DeleteExerciseTagAsync(input);

            var actual = await this.db.ExerciseTags.CountAsync();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(ExerciseId, 1, true)]
        [InlineData(ExerciseId, 0, false)]
        [InlineData(Invalid, 1, false)]
        [InlineData(Invalid, 0, false)]
        public async Task ExerciseTagByIdExistsWorksCorrectly(string exerciseId, int tagId, bool expected)
        {
            await this.SeedAsync();

            var actual = await this.tagService.ExerciseTagExistsAsync(exerciseId, tagId);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(ExerciseId, TagName, true)]
        [InlineData(ExerciseId, Invalid, false)]
        [InlineData(Invalid, TagName, false)]
        [InlineData(Invalid, Invalid, false)]
        public async Task ExerciseTagByNameExistsWorksCorrectly(string exerciseId, string name, bool expected)
        {
            await this.SeedAsync();

            var actual = await this.tagService.ExerciseTagExistsAsync(exerciseId, name);

            Assert.Equal(expected, actual);
        }

        private async Task SeedAsync()
        {
            var exerciseTag = new ExerciseTag
            {
                ExerciseId = ExerciseId,
                TagId = 1,
                Tag = new Tag
                {
                    Name = TagName,
                },
            };

            await this.db.AddAsync(exerciseTag);

            await this.db.SaveChangesAsync();

            this.db.Entry(exerciseTag).State = EntityState.Detached;
        }
    }
}
