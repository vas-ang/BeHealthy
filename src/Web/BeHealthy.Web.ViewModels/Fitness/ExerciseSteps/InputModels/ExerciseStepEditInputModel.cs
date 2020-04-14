namespace BeHealthy.Web.Dtos.Fitness.ExerciseSteps.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class ExerciseStepEditInputModel : IMapFrom<ExerciseStep>, IMapTo<ExerciseStep>
    {
        [Required]
        [StringLength(20, ErrorMessage = "{0} should be between {2} and {1} characters long", MinimumLength = 4)]
        public string Heading { get; set; }

        // TODO: think about data size!
        [Display(Name = "Image", Description = "If you don't want new image leave blank.")]
        [DataType(DataType.Upload)]
        public IFormFile ImageUpload { get; set; }

        [BindNever]
        public string Image { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "{0} should be between {2} and {1} characters long", MinimumLength = 20)]
        public string Description { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        [BindNever]
        public string ExerciseId { get; set; }
    }
}
