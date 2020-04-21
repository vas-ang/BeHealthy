namespace BeHealthy.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ExercisesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Exercises.Any())
            {
                return;
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == "angeloff");
            var admin = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == "angeloff.admin");

            var exercises = new List<Exercise>
            {
                new Exercise
                {
                    Name = "Band Bent-Over Row",
                    Creator = user,
                    Description = "You'll get used to the row in its many forms if you're working on your back — so start out with a light-resistance version that can serve as a warmup or a key part of your routine. The band will allow you to work through the range of motion without breaking out the weights, while still challenging you with some resistance.",
                    IsPublished = true,
                },
                new Exercise
                {
                    Name = "Renegade Row",
                    Creator = user,
                    Description = "The renegade row is all about maximizing the utility of a position to the highest degree. Take two high bang-for-your-buck moves, like the plank and pushup, and make them even useful by adding more elements to work different muscle groups. Work with light dumbbells here — maintaining the proper spinal position is just as important and rowing the weight.",
                    IsPublished = true,
                },
                new Exercise
                {
                    Name = "Dumbbell Single Arm Row",
                    Creator = user,
                    Description = "Dumbbell rows are a classic move that should have a place in every self-respecting lifter's heart. Your position perched on the bench will give your lats a chance to shine, while other rear-positioned muscles like the rhomboids and traps will kick in for support.",
                    IsPublished = true,
                },
                new Exercise
                {
                    Name = "Chest-Supported Dumbbell Row",
                    Creator = user,
                    Description = "If you struggle with keeping your chest strong and your spine straight when you try bent-over exercise variations, you'll love this move. The chest-supported row isolates your back and lets a bench do the work, allowing you to concentrate on moving the weight more efficiently.",
                    IsPublished = true,
                },
                new Exercise
                {
                    Name = "Dumbbell Squeeze Press",
                    Creator = user,
                    Description = "Squeezing the dumbbells together during a chest press moves the emphasis of the movement onto your pecs. This simple tweak engages them throughout the entire range of motion — a key factor to maximise muscle gain.",
                    IsPublished = true,
                },
                new Exercise
                {
                    Name = "Incline barbell bench press",
                    Creator = admin,
                    Description = "Pressing on an incline set-up works the clavicular head, which is why the incline barbell bench press makes your pecs pop.",
                    IsPublished = true,
                },
                new Exercise
                {
                    Name = "Unpublished exercise",
                    Creator = user,
                    Description = "This exercise is unpublished.",
                    IsPublished = false,
                },
                new Exercise
                {
                    Name = "Other user exercise",
                    Creator = admin,
                    Description = "Some other user exercise.",
                    IsPublished = true,
                },
            };

            await dbContext.Exercises.AddRangeAsync(exercises);
        }
    }
}
