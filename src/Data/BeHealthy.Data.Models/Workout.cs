namespace BeHealthy.Data.Models
{
    using System;
    using System.Collections.Generic;

    using BeHealthy.Data.Common.Models;
    using BeHealthy.Data.Models.Enumerators;

    public class Workout : BaseDeletableModel<string>
    {
        public Workout()
        {
            this.Id = Guid.NewGuid().ToString();
            this.WorkoutExercises = new HashSet<WorkoutExercise>();
        }

        public string Name { get; set; }

        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public Weekday Weekday { get; set; }

        public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
    }
}
