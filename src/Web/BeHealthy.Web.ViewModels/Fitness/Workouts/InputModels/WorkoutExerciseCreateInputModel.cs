namespace BeHealthy.Web.Dtos.Fitness.Workouts.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using BeHealthy.Web.Dtos.Fitness.Workouts.ViewModels;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class WorkoutExerciseCreateInputModel : IMapTo<WorkoutExercise>
    {
        [Required]
        [Display(Name = "Workout")]
        public string WorkoutId { get; set; }

        [Required]
        public string ExerciseId { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "{0} can be in range {1}-{2}")]
        public int Sets { get; set; }

        [Required]
        [Range(0, 500, ErrorMessage = "{0} can be in range {1}-{2})]")]
        public int Repetitions { get; set; }
    }
}
