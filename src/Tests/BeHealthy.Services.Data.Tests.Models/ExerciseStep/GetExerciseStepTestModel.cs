namespace BeHealthy.Services.Data.Tests.Models.ExerciseStep
{
    using BeHealthy.Services.Mapping;
    using BeHealthy.Data.Models;

    public class GetExerciseStepTestModel : IMapFrom<ExerciseStep>
    {
        public string Heading { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }
    }
}
