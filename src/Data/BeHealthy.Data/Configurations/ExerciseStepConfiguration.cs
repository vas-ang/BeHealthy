namespace BeHealthy.Data.Configurations
{
    using BeHealthy.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ExerciseStepConfiguration : IEntityTypeConfiguration<ExerciseStep>
    {
        public void Configure(EntityTypeBuilder<ExerciseStep> exStep)
        {
            exStep
                .Property(e => e.Description)
                .IsRequired(true);
        }
    }
}
