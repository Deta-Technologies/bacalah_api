namespace bacalah.API.Models;

public class CategoryTreeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public int DocumentsCount { get; set; }
    public List<CategoryTreeDto> SubCategories { get; set; } = new List<CategoryTreeDto>();
}