using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using main_service.Models;
using main_service.Models.DomainModels;
using Microsoft.IdentityModel.Tokens;


namespace main_service.Authentication;

/// <summary>
/// Used for JWT management, such as creating and validating tokens
/// </summary>
public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public Guid DecodeJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
        
        // TODO: Validate the token's signature
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"],
            ValidateLifetime = true
        };
        var decodedToken = tokenHandler.ReadJwtToken(token);
        var guid = decodedToken.Claims.First(claim => claim.Type == "guid").Value;
        var parsedGuid = Guid.Parse(guid);
        return parsedGuid;
    }
}

public class UserPrincipal
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Role { get; set; }
}