namespace BeHealthy.Web.Dtos.Reviews.ViewModels
{
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class ExerciseReviewViewModel : IMapFrom<ExerciseReview>
    {
        public string AuthorUserName { get; set; }

        public int Rating { get; set; }
    }
}
