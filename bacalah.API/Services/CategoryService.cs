using bacalah.API.Models;
using bacalah.Entities.Data;
using bacalah.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace bacalah.API.Services;

public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _context.Categories
                .Include(c => c.Parent)
                .Include(c => c.Documents)
                .Include(c => c.SubCategories)
                .ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                ParentName = c.Parent?.Name,
                SubCategoriesCount = c.SubCategories.Count,
                DocumentsCount = c.Documents.Count,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Parent)
                .Include(c => c.Documents)
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                ParentName = category.Parent?.Name,
                SubCategoriesCount = category.SubCategories.Count,
                DocumentsCount = category.Documents.Count,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
        }

        public async Task<List<CategoryTreeDto>> GetTreeAsync()
        {
            var categories = await _context.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.Documents)
                .Where(c => c.ParentId == null) // Start with root categories
                .ToListAsync();

            return categories.Select(c => MapToTreeDto(c)).ToList();
        }

        private CategoryTreeDto MapToTreeDto(Category category)
        {
            var dto = new CategoryTreeDto
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                DocumentsCount = category.Documents.Count,
                SubCategories = new List<CategoryTreeDto>()
            };

            foreach (var subCategory in category.SubCategories.OrderBy(sc => sc.Name))
            {
                dto.SubCategories.Add(MapToTreeDto(subCategory));
            }

            return dto;
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            // Validate parent exists if provided
            if (createCategoryDto.ParentId.HasValue)
            {
                var parentExists = await _context.Categories
                    .AnyAsync(c => c.Id == createCategoryDto.ParentId.Value);
                
                if (!parentExists)
                {
                    throw new ArgumentException("Parent category does not exist.");
                }
            }

            var category = new Category
            {
                Name = createCategoryDto.Name,
                ParentId = createCategoryDto.ParentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(category.Id) ?? throw new Exception("Failed to create category");
        }

        public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new ArgumentException("Category not found.");
            }

            // Prevent circular references
            if (updateCategoryDto.ParentId.HasValue)
            {
                if (updateCategoryDto.ParentId.Value == id)
                {
                    throw new ArgumentException("Category cannot be its own parent.");
                }

                // Check if new parent is a descendant (would create circular reference)
                if (await IsDescendantAsync(id, updateCategoryDto.ParentId.Value))
                {
                    throw new ArgumentException("Cannot set parent to a descendant category.");
                }

                var parentExists = await _context.Categories
                    .AnyAsync(c => c.Id == updateCategoryDto.ParentId.Value);
                
                if (!parentExists)
                {
                    throw new ArgumentException("Parent category does not exist.");
                }
            }

            category.Name = updateCategoryDto.Name;
            category.ParentId = updateCategoryDto.ParentId;
            category.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(category.Id) ?? throw new Exception("Failed to update category");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await CanDeleteAsync(id))
            {
                throw new InvalidOperationException("Cannot delete category that has subcategories or documents.");
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CanDeleteAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.Documents)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return false;

            return !category.SubCategories.Any() && !category.Documents.Any();
        }

        public async Task<List<CategoryDto>> GetSubCategoriesAsync(int parentId)
        {
            var categories = await _context.Categories
                .Where(c => c.ParentId == parentId)
                .Include(c => c.Documents)
                .Include(c => c.SubCategories)
                .ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                SubCategoriesCount = c.SubCategories.Count,
                DocumentsCount = c.Documents.Count,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();
        }

        private async Task<bool> IsDescendantAsync(int parentId, int potentialDescendantId)
        {
            var current = await _context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == potentialDescendantId);

            while (current != null)
            {
                if (current.ParentId == parentId) return true;
                if (current.ParentId == null) break;
                
                current = await _context.Categories
                    .Include(c => c.SubCategories)
                    .FirstOrDefaultAsync(c => c.Id == current.ParentId);
            }

            return false;
        }
    }