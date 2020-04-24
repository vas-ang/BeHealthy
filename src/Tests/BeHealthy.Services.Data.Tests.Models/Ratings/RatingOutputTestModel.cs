namespace BeHealthy.Services.Data.Tests.Models.Ratings
{
    using BeHealthy.Services.Mapping;
    using BeHealthy.Data.Models;

    public class RatingOutputTestModel : IMapFrom<ExerciseRating>
    {
        public int Rating { get; set; }

        public string ExerciseId { get; set; }
    }
}
