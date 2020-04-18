namespace BeHealthy.Web.Dtos.Fitness.Workouts.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeHealthy.Data.Models;
    using BeHealthy.Data.Models.Enumerators;
    using BeHealthy.Services.Mapping;

    public class WorkoutEditInputModel : IMapTo<Workout>, IMapFrom<Workout>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "{0} must be at least {2} and {1} at max characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public Weekday Weekday { get; set; }
    }
}
