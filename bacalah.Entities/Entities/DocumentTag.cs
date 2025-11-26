using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bacalah.Entities.Entities;

public class DocumentTag
{
    [Key, Column(Order = 0)]
    public int DocumentId { get; set; }

    [Key, Column(Order = 1)]
    public int TagId { get; set; }
    
    [ForeignKey("DocumentId")]
    public virtual Document Document { get; set; } = null!;

    [ForeignKey("TagId")]
    public virtual Tag Tag { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}