namespace BeHealthy.Web.Dtos.Fitness.Exercises.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class ExerciseInputModel : IMapTo<Exercise>
    {
        [Required]
        [StringLength(30, ErrorMessage = "{0} should be between {2} and {1} characters long.", MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "{0} should be max {1} characters long.")]
        public string Description { get; set; }
    }
}
