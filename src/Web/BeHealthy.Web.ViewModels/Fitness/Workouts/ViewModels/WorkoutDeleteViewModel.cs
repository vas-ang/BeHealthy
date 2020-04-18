namespace BeHealthy.Web.Dtos.Fitness.Workouts.ViewModels
{
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class WorkoutDeleteViewModel : IMapFrom<Workout>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Weekday { get; set; }
    }
}
