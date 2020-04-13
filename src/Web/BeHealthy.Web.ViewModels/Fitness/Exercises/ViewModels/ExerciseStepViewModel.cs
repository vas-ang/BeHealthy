namespace BeHealthy.Web.Dtos.Fitness.Exercises.ViewModels
{
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class ExerciseStepViewModel : IMapFrom<ExerciseStep>
    {
        public string Heading { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }
    }
}
