using main_service.Models.DtoModels;

namespace main_service.Models.ApiModels.UserDetailApiModels;

public class GetUserDetailsResponse
{
    public int TotalUserDetails { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public string Search { get; set; } = null!;
    public string Sort { get; set; } = null!;
    public List<UserDetailsDto> UserDetails { get; set; } = null!;
}