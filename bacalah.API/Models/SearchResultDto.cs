namespace bacalah.API.Models;

public class SearchResultDto
{
    public List<DocumentListDto> Documents { get; set; } = new List<DocumentListDto>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}