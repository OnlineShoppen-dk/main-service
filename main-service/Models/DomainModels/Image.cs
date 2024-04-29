using System.ComponentModel.DataAnnotations;

namespace main_service.Models.DomainModels;

public class Image
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string FileName { get; set; } = null!;
    [Required]
    public string Alt { get; set; } = null!; 
}