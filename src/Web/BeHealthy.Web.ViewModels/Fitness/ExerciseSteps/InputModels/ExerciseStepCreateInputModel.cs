namespace BeHealthy.Web.Dtos.Fitness.ExerciseSteps.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using BeHealthy.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;

    using FileExtensions = BeHealthy.Web.Infrastructure.ValidationAttributes.FileExtensionsAttribute;

    public class ExerciseStepCreateInputModel : IMapTo<ExerciseStep>
    {
        [Required]
        [StringLength(20, ErrorMessage = "{0} should be between {2} and {1} characters long", MinimumLength = 4)]
        public string Heading { get; set; }

        [Required]
        [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        [FileSize(4 * 1024 * 1024, "{0} size must be at max 4MB")]
        [FileExtensions(errorMessage: "Unallowed file extension. Allowed extensions: {2}", allowedExtensions: new string[] { ".png", ".jpg", ".jpeg" })]
        public IFormFile ImageUpload { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "{0} should be between {2} and {1} characters long", MinimumLength = 20)]
        public string Description { get; set; }

        [Required]
        public string ExerciseId { get; set; }
    }
}
