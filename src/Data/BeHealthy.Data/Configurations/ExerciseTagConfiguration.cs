namespace BeHealthy.Data.Configurations
{
    using BeHealthy.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ExerciseTagConfiguration : IEntityTypeConfiguration<ExerciseTag>
    {
        public void Configure(EntityTypeBuilder<ExerciseTag> exTag)
        {
            exTag
                .HasKey(e => new { e.ExerciseId, e.TagId });
        }
    }
}
