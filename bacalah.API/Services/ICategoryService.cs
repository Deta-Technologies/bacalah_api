using bacalah.API.Models;

namespace bacalah.API.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<List<CategoryTreeDto>> GetTreeAsync();
    Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto);
    Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto);
    Task<bool> DeleteAsync(int id);
    Task<bool> CanDeleteAsync(int id);
    Task<List<CategoryDto>> GetSubCategoriesAsync(int parentId);
}