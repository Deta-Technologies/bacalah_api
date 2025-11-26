using bacalah.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bacalah.Entities.EntityConfigurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasIndex(t => t.Name)
            .IsUnique();
        builder.HasIndex(t => t.CreatedAt);
    }
}