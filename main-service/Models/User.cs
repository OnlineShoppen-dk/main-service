namespace main_service.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Role { get; set; } = UserRoles.User;
    
}

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
}
