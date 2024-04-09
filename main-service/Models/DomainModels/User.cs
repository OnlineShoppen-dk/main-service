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
}