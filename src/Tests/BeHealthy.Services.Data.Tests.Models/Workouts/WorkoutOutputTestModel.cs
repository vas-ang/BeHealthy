namespace BeHealthy.Services.Data.Tests.Models.Workouts
{
    using BeHealthy.Services.Mapping;
    using BeHealthy.Data.Models;
    using BeHealthy.Data.Models.Enumerators;

    public class WorkoutOutputTestModel : IMapFrom<Workout>
    {
        public string Name { get; set; }

        public Weekday Weekday { get; set; }
    }
}
