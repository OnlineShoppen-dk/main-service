using System.ComponentModel.DataAnnotations;
using main_service.Models.DomainModels.ProductDomainModels;

namespace main_service.Models.DomainModels;

public class Image
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string FileName { get; set; } = null!;
    [Required]
    public string Alt { get; set; } = null!; 
    
    // Relations to other entities
    public int? ProductId { get; set; }
    public Product Product { get; set; } = null!;
}