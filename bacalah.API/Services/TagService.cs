using bacalah.API.Models;
using bacalah.Entities.Data;
using bacalah.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace bacalah.API.Services;

public class TagService : ITagService
{
    private readonly ApplicationDbContext _dbContext;
    
    public TagService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TagDto>> GetAllAsync()
    {
        var tags = await _dbContext.Tags
            .Include(t => t.DocumentTags)
            .OrderBy(t => t.Name)
            .ToListAsync();

        return tags.Select(t => new TagDto
        {
            Id = t.Id,
            Name = t.Name,
            DocumentsCount = t.DocumentTags.Count,
            CreatedAt = t.CreatedAt,
        }).ToList();
    }

    public async Task<TagDto?> GetByIdAsync(int id)
    {
        var tags = await _dbContext.Tags
            .Include(t => t.DocumentTags)
            .FirstOrDefaultAsync(t => t.Id == id);
        
        if (tags == null) return null;

        return new TagDto
        {
            Id = tags.Id,
            Name = tags.Name,
            DocumentsCount = tags.DocumentTags.Count,
            CreatedAt = tags.CreatedAt,
        };
    }

    public async Task<TagDto?> GetByNameAsync(string name)
    {
        var tags = await _dbContext.Tags
            .Include(t => t.DocumentTags)
            .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
        
        if (tags == null) return null;

        return new TagDto
        {
            Id = tags.Id,
            Name = tags.Name,
            DocumentsCount = tags.DocumentTags.Count,
            CreatedAt = tags.CreatedAt,
        };
    }

    public async Task<TagDto> CreateAsync(CreateTagDto createTagDto)
    {
        var normalizedName = createTagDto.Name.Trim();

        var existingTag = await GetByNameAsync(normalizedName);
        if (existingTag != null)
            throw new Exception("Tag with the same name already exists.");

        var tag = new Tag
        {
            Name = normalizedName,
            CreatedAt = DateTime.UtcNow
        };
        
        _dbContext.Add(tag);
        await _dbContext.SaveChangesAsync();
        
        return await GetByIdAsync(tag.Id) ?? throw new Exception("Failed to create tag");
    }

    public async Task<TagDto> UpdateAsync(int id, UpdateTagDto updateTagDto)
    {
        var tag = await _dbContext.Tags.FindAsync(id);
        if (tag == null) throw new ArgumentException("Tag not found.");
        
        var normalizedName = updateTagDto.Name.Trim();
        var existingTag = await _dbContext.Tags
            .FirstOrDefaultAsync(t => t.Name.ToLower() == normalizedName.ToLower() && t.Id != id);
        if (existingTag != null)
            throw new Exception("Tag with the same name already exists.");
        
        tag.Name = normalizedName;
        await _dbContext.SaveChangesAsync();
        
        return await GetByIdAsync(tag.Id) ?? throw new Exception("Failed to update tag");
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tag = await _dbContext.Tags
            .Include(t => t.DocumentTags)
            .FirstOrDefaultAsync(t => t.Id == id);
        
        if (tag == null)
            return false;
        
        if (tag.DocumentTags.Any())
        {
            throw new InvalidOperationException("Cannot delete tag that is associated with documents.");
        }
        
        _dbContext.Tags.Remove(tag);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<TagDto>> GetPopularAsync(int count = 10)
    {
        var tags = await _dbContext.Tags
            .Include(t => t.DocumentTags)
            .OrderByDescending(t => t.DocumentTags.Count)
            .ThenBy(t => t.Name)
            .Take(count)
            .ToListAsync();

        return tags.Select(t => new TagDto
        {
            Id = t.Id,
            Name = t.Name,
            DocumentsCount = t.DocumentTags.Count,
            CreatedAt = t.CreatedAt
        }).ToList();
    }
}