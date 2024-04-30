namespace main_service.Models.ApiModels.UserDetailApiModels;

public class RegisterNewUserRequest
{
    // Required fields
    public Guid Guid { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    
    // Optional fields
    public string? PhoneNumber { get; set; }
}