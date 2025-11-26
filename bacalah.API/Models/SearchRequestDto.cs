namespace bacalah.API.Models;

public class SearchRequestDto
{
    public string? Query { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public List<int> TagIds { get; set; } = new List<int>();
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "UpdatedAt";
    public bool SortDescending { get; set; } = true;
}