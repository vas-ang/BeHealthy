namespace BeHealthy.Data.Configurations
{
    using BeHealthy.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class WorkoutExerciseConfiguration : IEntityTypeConfiguration<WorkoutExercise>
    {
        public void Configure(EntityTypeBuilder<WorkoutExercise> workoutExercise)
        {
            workoutExercise
                .HasKey(x => new { x.ExerciseId, x.WorkoutId });
        }
    }
}
