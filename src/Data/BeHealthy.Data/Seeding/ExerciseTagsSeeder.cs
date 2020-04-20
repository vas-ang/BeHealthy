namespace BeHealthy.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ExerciseTagsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ExerciseTags.Any())
            {
                return;
            }

            var backExerciseNames = new string[] { "Band Bent-Over Row", "Renegade Row", "Dumbbell Single Arm Row", "Chest-Supported Dumbbell Row", };

            var chestExerciseNames = new string[] { "Dumbbell Squeeze Press", "Incline barbell bench press", "Other user exercise", };

            var backExercises = await dbContext.Exercises.Where(x => backExerciseNames.Contains(x.Name)).ToArrayAsync();
            var chestExercises = await dbContext.Exercises.Where(x => chestExerciseNames.Contains(x.Name)).ToArrayAsync();

            var backTag = await dbContext.Tags.FirstOrDefaultAsync(x => x.Name == "back");
            var chestTag = await dbContext.Tags.FirstOrDefaultAsync(x => x.Name == "chest");
            var upperBodyTag = await dbContext.Tags.FirstOrDefaultAsync(x => x.Name == "upper-body");

            foreach (var backExercise in backExercises)
            {
                await dbContext.AddAsync(new ExerciseTag { Exercise = backExercise, Tag = backTag });
                await dbContext.AddAsync(new ExerciseTag { Exercise = backExercise, Tag = upperBodyTag });
            }

            foreach (var chestExercise in chestExercises)
            {
                await dbContext.AddAsync(new ExerciseTag { Exercise = chestExercise, Tag = chestTag });
                await dbContext.AddAsync(new ExerciseTag { Exercise = chestExercise, Tag = upperBodyTag });
            }
        }
    }
}
