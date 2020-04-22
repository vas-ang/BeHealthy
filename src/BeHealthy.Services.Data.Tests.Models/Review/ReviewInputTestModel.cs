namespace BeHealthy.Services.Data.Tests.Models.Review
{
    using BeHealthy.Services.Mapping;
    using BeHealthy.Data.Models;

    public class ReviewInputTestModel : IMapTo<ExerciseReview>
    {
        public int Rating { get; set; }

        public string ExerciseId { get; set; }
    }
}
