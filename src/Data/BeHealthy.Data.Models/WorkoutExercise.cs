namespace BeHealthy.Data.Models
{
    public class WorkoutExercise
    {
        public string ExerciseId { get; set; }

        public Exercise Exercise { get; set; }

        public string WorkoutId { get; set; }

        public Workout Workout { get; set; }

        public int Sets { get; set; }

        public int Repetitions { get; set; }
    }
}
