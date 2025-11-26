using bacalah.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bacalah.Entities.EntityConfigurations;

public class DocumentTagConfiguration : IEntityTypeConfiguration<DocumentTag>
{
    public void Configure(EntityTypeBuilder<DocumentTag> builder)
    {
        builder.HasKey(t => new { t.DocumentId, t.TagId });
        builder.HasOne(dt => dt.Document)
            .WithMany(t => t.DocumentTags)
            .HasForeignKey(dt => dt.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(dt => dt.DocumentId);
        builder.HasIndex(dt => dt.TagId);
        builder.HasIndex(dt => dt.CreatedAt);
        
    }
}