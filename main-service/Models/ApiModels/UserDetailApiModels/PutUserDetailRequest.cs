namespace main_service.Models.ApiModels.UserDetailApiModels;

public class PutUserDetailRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}