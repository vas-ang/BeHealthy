namespace BeHealthy.Services.Data.Tests.Models.Workouts
{
    using BeHealthy.Services.Mapping;
    using BeHealthy.Data.Models;
    using BeHealthy.Data.Models.Enumerators;

    public class WorkoutInputTestModel : IMapTo<Workout>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Weekday Weekday { get; set; }
    }
}
