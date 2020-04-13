namespace BeHealthy.Web.Dtos.Fitness.ExerciseSteps.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class ExerciseStepInputModel : IMapTo<ExerciseStep>
    {
        [Required]
        [StringLength(20, ErrorMessage = "{0} should be between {2} and {1} characters long", MinimumLength = 4)]
        public string Heading { get; set; }

        // TODO: think about data size!
        [Required]
        [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        public IFormFile ImageUpload { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "{0} should be between {2} and {1} characters long", MinimumLength = 20)]
        public string Description { get; set; }

        [Required]
        public string ExerciseId { get; set; }
    }
}
