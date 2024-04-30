namespace main_service.Models.ApiModels.UserDetailApiModels;

public class ChangePasswordRequest
{
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}