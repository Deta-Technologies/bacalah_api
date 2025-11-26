using System.ComponentModel.DataAnnotations;

namespace bacalah.API.Models;

public class UpdateDocumentDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public int? CategoryId { get; set; }
        
    public List<string> Tags { get; set; } = new List<string>();
}