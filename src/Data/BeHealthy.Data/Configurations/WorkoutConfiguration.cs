namespace BeHealthy.Data.Configurations
{
    using BeHealthy.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
    {
        public void Configure(EntityTypeBuilder<Workout> workout)
        {
            workout
                .Property(x => x.Name)
                .IsRequired(true);
        }
    }
}
