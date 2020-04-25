namespace BeHealthy.Services.Data.Tests.Models.ExerciseSteps
{
    using BeHealthy.Services.Mapping;
    using BeHealthy.Data.Models;

    public class CreateExerciseStepTestModel : IMapTo<ExerciseStep>
    {
        public int Id { get; set; }

        public string Heading { get; set; }

        public string Description { get; set; }

        public string ExerciseId { get; set; }
    }
}
