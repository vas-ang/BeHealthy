namespace BeHealthy.Web.Dtos.Tags.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class ExerciseTagCreateInputModel
    {
        [Required]
        public string ExerciseId { get; set; }

        [Required]
        [RegularExpression("[a-z\\-]+", ErrorMessage = "{0} must contain only lowercase letters and '-'!")]
        [StringLength(10, ErrorMessage = "{0} must be at least {2} and max {1} characters long and should not contain spaces!", MinimumLength = 3)]
        public string TagName { get; set; }
    }
}
