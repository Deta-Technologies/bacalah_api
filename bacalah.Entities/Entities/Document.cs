using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bacalah.Entities.Entities;

[Table("documents")]
public class Document
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty; // Markdown content
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
    
    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }
    
    public virtual ICollection<DocumentTag> DocumentTags { get; set; } = new List<DocumentTag>();
    
    [NotMapped]
    public ICollection<string> TagNames => DocumentTags.Select(dt => dt.Tag.Name).ToList();
}