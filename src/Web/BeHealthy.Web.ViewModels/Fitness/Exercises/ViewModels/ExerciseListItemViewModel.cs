namespace BeHealthy.Web.Dtos.Fitness.Exercises.ViewModels
{
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class ExerciseListItemViewModel : IMapFrom<Exercise>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CreatorUserName { get; set; }
    }
}
