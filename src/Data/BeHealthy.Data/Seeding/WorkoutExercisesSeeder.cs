namespace BeHealthy.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class WorkoutExercisesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.WorkoutExercises.Any())
            {
                return;
            }

            var backExerciseNames = new string[] { "Band Bent-Over Row", "Renegade Row", "Dumbbell Single Arm Row", "Chest-Supported Dumbbell Row", };

            var chestExerciseNames = new string[] { "Dumbbell Squeeze Press", "Incline barbell bench press", "Other user exercise", };

            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == "angeloff");

            var random = new Random(0);

            var backWorkout = await dbContext.Workouts.FirstOrDefaultAsync(x => x.Name == "Back");
            var chestWorkout = await dbContext.Workouts.FirstOrDefaultAsync(x => x.Name == "Chest");

            var backExercises = await dbContext.Exercises.Where(x => backExerciseNames.Contains(x.Name)).Select(x => new WorkoutExercise { Exercise = x, Repetitions = random.Next(0, 13), Sets = random.Next(2, 6), Workout = backWorkout }).ToArrayAsync();
            var chestExercises = await dbContext.Exercises.Where(x => chestExerciseNames.Contains(x.Name)).Select(x => new WorkoutExercise { Exercise = x, Repetitions = random.Next(0, 13), Sets = random.Next(2, 6), Workout = chestWorkout }).ToArrayAsync();

            await dbContext.WorkoutExercises.AddRangeAsync(backExercises);
            await dbContext.WorkoutExercises.AddRangeAsync(chestExercises);
        }
    }
}
