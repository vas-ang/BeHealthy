namespace BeHealthy.Data.Configurations
{
    using BeHealthy.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ExerciseReviewConfiguration : IEntityTypeConfiguration<ExerciseReview>
    {
        public void Configure(EntityTypeBuilder<ExerciseReview> exReview)
        {
            exReview.HasKey(e => new { e.AuthorId, e.ExerciseId });
        }
    }
}
