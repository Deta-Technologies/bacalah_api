namespace bacalah.API.Models;

public class TagDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int DocumentCount { get; set; }
}