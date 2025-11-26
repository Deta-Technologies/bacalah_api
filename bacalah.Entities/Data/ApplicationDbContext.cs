using bacalah.Entities.Entities;
using bacalah.Entities.EntityConfigurations;
using bacalah.Entities.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace bacalah.Entities.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Document> Documents { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<DocumentTag> DocumentTags { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new DocumentConfiguration());
        builder.ApplyConfiguration(new TagConfiguration());
        builder.ApplyConfiguration(new DocumentTagConfiguration());

        SeedData.Initialize(builder);
    }
    
    // For SeedData to auto update UpdatedAt
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Automatically update UpdatedAt timestamp for entities that have it
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Category || e.Entity is Document)
            .Where(e => e.State == EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            if (entityEntry.Entity is Category category)
            {
                category.UpdatedAt = DateTime.UtcNow;
            }
            else if (entityEntry.Entity is Document document)
            {
                document.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}