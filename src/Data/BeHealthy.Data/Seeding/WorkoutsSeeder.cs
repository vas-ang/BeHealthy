namespace BeHealthy.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using BeHealthy.Data.Models.Enumerators;
    using Microsoft.EntityFrameworkCore;

    public class WorkoutsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Workouts.Any())
            {
                return;
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == "angeloff");

            var workouts = new List<Workout>
            {
                new Workout
                {
                    Name = "Chest",
                    Creator = user,
                    Weekday = Weekday.Monday,
                },
                new Workout
                {
                    Name = "Back",
                    Creator = user,
                    Weekday = Weekday.Wednesday,
                },
            };

            await dbContext.Workouts.AddRangeAsync(workouts);
        }
    }
}
