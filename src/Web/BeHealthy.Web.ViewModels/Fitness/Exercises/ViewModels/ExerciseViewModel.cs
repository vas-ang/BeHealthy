namespace BeHealthy.Web.Dtos.Fitness.Exercises.ViewModels
{
    using System.Collections.Generic;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class ExerciseViewModel : IMapFrom<Exercise>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPublished { get; set; }

        public bool IsAcessorCreator { get; set; }

        public string CreatorUserName { get; set; }

        public IEnumerable<ExerciseStepViewModel> ExerciseSteps { get; set; }

        public IEnumerable<ExerciseTagViewModel> ExerciseTags { get; set; }
    }
}
