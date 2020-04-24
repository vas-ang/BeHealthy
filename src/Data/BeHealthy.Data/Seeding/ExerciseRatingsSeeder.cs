namespace BeHealthy.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ExerciseRatingsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ExerciseRatings.Any())
            {
                return;
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == "angeloff.spectator");

            var exercises = await dbContext.Exercises.Where(x => x.IsPublished).ToArrayAsync();

            var random = new Random(0);

            foreach (var exercise in exercises)
            {
                await dbContext.ExerciseRatings.AddAsync(new ExerciseRating
                {
                    User = user,
                    Exercise = exercise,
                    Rating = random.Next(1, 6),
                });
            }
        }
    }
}
