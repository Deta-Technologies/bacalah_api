using bacalah.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace bacalah.Entities.Seed;

public static class SeedData
{
    public static void  Initialize(ModelBuilder builder)
    {
        // Seed root categories
        var rootCategory1 = new Category { Id = 1, Name = "Technology", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var rootCategory2 = new Category { Id = 2, Name = "Science", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var subCategory1 = new Category { Id = 3, Name = "Programming", ParentId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var subCategory2 = new Category { Id = 4, Name = "Database", ParentId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

        builder.Entity<Category>().HasData(rootCategory1, rootCategory2, subCategory1, subCategory2);

        // Seed some common tags
        var commonTags = new[]
        {
            new Tag { Id = 1, Name = "tutorial", CreatedAt = DateTime.UtcNow },
            new Tag { Id = 2, Name = "guide", CreatedAt = DateTime.UtcNow },
            new Tag { Id = 3, Name = "advanced", CreatedAt = DateTime.UtcNow },
            new Tag { Id = 4, Name = "beginner", CreatedAt = DateTime.UtcNow },
            new Tag { Id = 5, Name = "reference", CreatedAt = DateTime.UtcNow }
        };

        builder.Entity<Tag>().HasData(commonTags);
    }
}