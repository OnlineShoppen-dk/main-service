using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace main_service.Configurations;

/// <summary>
/// This class is used to configure the authentication for the application
///  - The issuer is the server that created the token
///  - The audience is the intended recipient of the token
///  - The key is the secret key used to sign the token
/// </summary>
public static class AuthenticationConfig
{
    public static void Configure(IServiceCollection services, string issuer, string audience, string key)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });
    }
}