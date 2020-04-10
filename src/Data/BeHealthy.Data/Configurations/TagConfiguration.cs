namespace BeHealthy.Data.Configurations
{
    using BeHealthy.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> tag)
        {
            tag
                .Property(e => e.Name)
                .IsRequired(true);
        }
    }
}
