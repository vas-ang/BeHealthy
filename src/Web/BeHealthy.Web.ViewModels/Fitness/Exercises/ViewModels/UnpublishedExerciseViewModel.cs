namespace BeHealthy.Web.Dtos.Fitness.Exercises.ViewModels
{
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class UnpublishedExerciseViewModel : IMapFrom<Exercise>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
