using System.ComponentModel.DataAnnotations;

namespace bacalah.API.Models;

public class UpdateTagDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
}