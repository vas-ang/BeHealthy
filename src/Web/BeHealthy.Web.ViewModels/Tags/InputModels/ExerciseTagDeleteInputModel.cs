namespace BeHealthy.Web.Dtos.Tags.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class ExerciseTagDeleteInputModel : IMapTo<ExerciseTag>
    {
        [Required]
        public string ExerciseId { get; set; }

        [Required]
        public int TagId { get; set; }
    }
}
