namespace BeHealthy.Data.Configurations
{
    using BeHealthy.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> ex)
        {
            ex
                .Property(e => e.Name)
                .IsRequired(true);

            ex
                .Property(e => e.Description)
                .IsRequired(true);

            ex
                .Property(e => e.CreatorId)
                .IsRequired(true);
        }
    }
}
