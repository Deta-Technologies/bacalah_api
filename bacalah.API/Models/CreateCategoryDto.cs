using System.ComponentModel.DataAnnotations;

namespace bacalah.API.Models;

public class CreateCategoryDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public int? ParentId { get; set; }
}