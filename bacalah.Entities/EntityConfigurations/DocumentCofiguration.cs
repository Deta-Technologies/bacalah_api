using bacalah.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bacalah.Entities.EntityConfigurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(d => d.Category)
            .WithMany(c => c.Documents)
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(d => d.Title);
        builder.HasIndex(d => d.CreatedAt);
        builder.HasIndex(d => d.UserId);
        builder.HasIndex(d => d.CategoryId);
        builder.Property(d => d.Content)
            .HasColumnType("LONGTEXT"); // For MySQL, supports large markdown content
    }
}