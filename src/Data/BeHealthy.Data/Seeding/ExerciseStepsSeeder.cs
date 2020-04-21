namespace BeHealthy.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ExerciseStepsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ExerciseSteps.Any())
            {
                return;
            }

            var inclineBarbellBenchPress = await dbContext.Exercises.FirstOrDefaultAsync(x => x.Name == "Incline barbell bench press");
            var dumbbellSqueezePress = await dbContext.Exercises.FirstOrDefaultAsync(x => x.Name == "Dumbbell Squeeze Press");

            var exerciseSteps = new List<ExerciseStep>
            {
                new ExerciseStep
                {
                    Heading = "Push up",
                    Image = "https://res.cloudinary.com/dk14bw88t/image/upload/v1587413988/angeloff/xucve2ne99wtuxzksckv.png",
                    Description = "Bend your arms and lower them to the side of your body so the dumbbells lie just above your chest. Pause and then lift your arms to repeat.",
                    Exercise = inclineBarbellBenchPress,
                },
                new ExerciseStep
                {
                    Heading = "Make the right pose",
                    Image = "https://res.cloudinary.com/dk14bw88t/image/upload/v1587413945/angeloff/ofcedppungogvyvqnhut.png",
                    Description = "Lie on a flat bench and hold a dumbbell in each hand. Maintain a neutral grip and begin with your arms straight, directly above you.",
                    Exercise = inclineBarbellBenchPress,
                },
                new ExerciseStep
                {
                    Heading = "Execution",
                    Image = "https://res.cloudinary.com/dk14bw88t/image/upload/v1587414190/angeloff/eyms1pxlezar4dtsued4.png",
                    Description = "Bend your arms and lower them to the side of your body so the dumbbells lie just above your chest. Pause and then lift your arms to repeat.",
                    Exercise = dumbbellSqueezePress,
                },
                new ExerciseStep
                {
                    Heading = "Make the right pose",
                    Image = "https://res.cloudinary.com/dk14bw88t/image/upload/v1587414118/angeloff/qks84tplr0fszzp4iqic.png",
                    Description = "Lie on a flat bench and hold a dumbbell in each hand. Maintain a neutral grip and begin with your arms straight, directly above you.",
                    Exercise = dumbbellSqueezePress,
                },
            };

            await dbContext.AddRangeAsync(exerciseSteps);
        }
    }
}
