namespace BeHealthy.Data.Configurations
{
    using BeHealthy.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ExerciseRatingConfiguration : IEntityTypeConfiguration<ExerciseRating>
    {
        public void Configure(EntityTypeBuilder<ExerciseRating> exerciseRating)
        {
            exerciseRating.HasKey(e => new { e.UserId, e.ExerciseId });
        }
    }
}
