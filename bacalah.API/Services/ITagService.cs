using bacalah.API.Models;

namespace bacalah.API.Services;

public interface ITagService
{
    Task<List<TagDto>> GetAllAsync();
    Task<TagDto?> GetByIdAsync(int id);
    Task<TagDto?> GetByNameAsync(string name);
    Task<TagDto> CreateAsync(CreateTagDto createTagDto);
    Task<TagDto> UpdateAsync(int id, UpdateTagDto updateTagDto);
    Task<bool> DeleteAsync(int id);
    Task<List<TagDto>> GetPopularAsync(int count = 10);
}