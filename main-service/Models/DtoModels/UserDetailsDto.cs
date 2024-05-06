using main_service.Models.Representation;

namespace main_service.Models.DtoModels;

public class UserDetailsDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    public UserDetailsRepresentation ToRepresentation()
    {
        return new UserDetailsRepresentation
        {
            id = Id,
            firstName = FirstName,
            lastName = LastName,
            email = Email,
            phoneNumber = PhoneNumber
        };
    }
}