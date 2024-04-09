using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace main_service.Models.DomainModels;

[Table("products")]
[Index(nameof(Name), IsUnique = true)]
public class Product
{
    public int Id { get; set; }
    // A Guid for each product?
    // public Guid Guid { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal? Price { get; set; } = 0;
    public int? Stock { get; set; } = 0;
    public int? Sold { get; set; } = 0;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    /*
    - - DISCOUNT
    public int Discount { get; set; }
    public decimal DiscountedPrice => (decimal)(Price - (Price * Discount / 100))!;
    
    - - RATING
    public decimal AverageRating { get; set; }
    public int RatingCount => Ratings.Count;
    public List<Rating> Ratings { get; set; } = new();
    
    -- IMAGE
    public int? ThumbnailId { get; set; }
    public List<Image> Images { get; set; } = new();
    */
    
}