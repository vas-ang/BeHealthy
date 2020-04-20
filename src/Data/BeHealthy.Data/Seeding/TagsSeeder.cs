namespace BeHealthy.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;

    public class TagsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Tags.Any())
            {
                return;
            }

            var tags = new List<Tag>
            {
                new Tag
                {
                Name = "back",
                },
                new Tag
                {
                Name = "chest",
                },
                new Tag
                {
                Name = "upper-body",
                },
            };

            await dbContext.Tags.AddRangeAsync(tags);
        }
    }
}
