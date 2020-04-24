namespace BeHealthy.Web.Dtos.Ratings.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class ExerciseRatingInputModel : IMapTo<ExerciseRating>
    {
        [Required]
        public string ExerciseId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
