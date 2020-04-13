namespace BeHealthy.Web.Dtos.Fitness.Exercises.ViewModels
{
    using System.Collections.Generic;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class ExerciseEditViewModel : IMapFrom<Exercise>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<ExerciseStepViewModel> ExerciseSteps { get; set; }
    }
}
