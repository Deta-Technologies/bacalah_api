using System.ComponentModel.DataAnnotations;

namespace bacalah.Entities.Entities;

public class Tag
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    public virtual ICollection<DocumentTag> DocumentTags { get; set; } = new List<DocumentTag>();
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public int DocumentCount => DocumentTags?.Count ?? 0;
}