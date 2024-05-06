namespace main_service.Models.Representation;

public class UserDetailsRepresentation
{
    public int id { get; set; }
    public string firstName { get; set; } = null!;
    public string lastName { get; set; } = null!;
    public string email { get; set; } = null!;
    public string phoneNumber { get; set; } = null!;
}