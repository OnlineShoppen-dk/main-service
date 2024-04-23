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

    public static UserPrincipal GetUser(ClaimsPrincipal claims)
    {
        var user = new UserPrincipal
        {
            Id = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            Name = claims.FindFirst(ClaimTypes.GivenName)!.Value,
            Role = claims.FindFirst(ClaimTypes.Role)!.Value,
        };
        return user;
    }

    public string GenerateToken(UserDetails userDetails)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userDetails.Id.ToString()!),
            // new(ClaimTypes.GivenName, user.Name!),
            // new(ClaimTypes.Role, user.Role),
        };
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddDays(30),
            Issuer = "main-service",
            Audience = "main-service",
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(authClaims)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class UserPrincipal
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Role { get; set; } = null!;
}