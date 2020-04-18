namespace BeHealthy.Web.Dtos.Fitness.Workouts.ViewModels
{
    using System.Collections.Generic;

    using BeHealthy.Data.Models;
    using BeHealthy.Data.Models.Enumerators;
    using BeHealthy.Services.Mapping;

    public class WorkoutDetailsViewModel : IMapFrom<Workout>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Weekday Weekday { get; set; }

        public IEnumerable<WorkoutExerciseViewModel> WorkoutExercises { get; set; }
    }
}
