namespace bacalah.API.Models;

public class DocumentListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ContentPreview { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public List<string> TagNames { get; set; } = new List<string>();
}