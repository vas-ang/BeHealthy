namespace BeHealthy.Web.Dtos.Ratings.ViewModels
{
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class ExerciseRatingViewModel : IMapFrom<ExerciseRating>
    {
        public string AuthorUserName { get; set; }

        public int Rating { get; set; }
    }
}
