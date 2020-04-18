namespace BeHealthy.Web.Dtos.Fitness.Workouts.ViewModels
{
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class WorkoutExerciseViewModel : IMapFrom<WorkoutExercise>
    {
        public string ExerciseId { get; set; }

        public string ExerciseName { get; set; }

        public string ExerciseDescription { get; set; }

        public int Sets { get; set; }

        public int Repetitions { get; set; }
    }
}
