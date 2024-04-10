using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace main_service.Models.DomainModels;

public class User
{
    
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}

public class UserRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
}