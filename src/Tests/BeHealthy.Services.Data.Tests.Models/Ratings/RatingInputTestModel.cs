namespace BeHealthy.Services.Data.Tests.Models.Ratings
{
    using BeHealthy.Services.Mapping;
    using BeHealthy.Data.Models;

    public class RatingInputTestModel : IMapTo<ExerciseRating>
    {
        public int Rating { get; set; }

        public string ExerciseId { get; set; }
    }
}
