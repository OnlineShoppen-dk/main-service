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
        var issuer = Environment.GetEnvironmentVariable("ISSUER") ?? _configuration["Jwt:Issuer"];
        var audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? _configuration["Jwt:Audience"];
        var key = Environment.GetEnvironmentVariable("KEY") ?? _configuration["Jwt:Key"];
        if (issuer is null || audience is null || key is null)
        {
            throw new Exception("Missing configuration in JwtHelper");
        }
        var tokenHandler = new JwtSecurityTokenHandler();
        var keyBytes = Encoding.ASCII.GetBytes(key);
        // TODO: Validate the token's signature
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
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