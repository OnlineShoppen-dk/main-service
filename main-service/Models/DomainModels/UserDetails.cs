using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace main_service.Models.DomainModels;

[Index(nameof(Email), IsUnique = true)]
public class UserDetails
{
    [Key]
    public int Id { get; set; }
    
    public Guid Guid { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;
}

public class UserRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
}