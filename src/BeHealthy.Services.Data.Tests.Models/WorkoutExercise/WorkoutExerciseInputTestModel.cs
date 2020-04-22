namespace BeHealthy.Services.Data.Tests.Models.WorkoutExercise
{
    using BeHealthy.Services.Mapping;
    using BeHealthy.Data.Models;

    public class WorkoutExerciseInputTestModel : IMapTo<WorkoutExercise>
    {
        public string ExerciseId { get; set; }

        public string WorkoutId { get; set; }

        public int Sets { get; set; }

        public int Repetitions { get; set; }
    }
}
