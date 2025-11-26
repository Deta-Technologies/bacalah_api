using bacalah.API.Models;

namespace bacalah.API.Services;

public interface IDocumentService
{
    Task<PaginatedResult<DocumentListDto>> GetDocumentsAsync(string userId, int pageNumber, int pageSize = 10);
    Task<DocumentDto?> GetByIdAsync(int id, string userId);
    Task<PaginatedResult<DocumentListDto>> GetByCategoryAsync(int? categoryId, string userId, int pageNumber = 1, int pageSize = 10);
    Task<DocumentDto> CreateAsync(CreateDocumentDto createDocumentDto, string userId);
    Task<DocumentDto> UpdateAsync(int id, UpdateDocumentDto updateDocumentDto, string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task<List<DocumentListDto>> GetRecentAsync(string userId, int count = 5);
}

public class PaginatedResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}