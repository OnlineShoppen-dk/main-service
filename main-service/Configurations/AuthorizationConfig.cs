using main_service.Authentication.Policies;
using main_service.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace main_service.Configurations;

/// <summary>
/// This class is used to configure the authorization policies
///  - IsAdmin policy requires the user to be an admin
///  - IsUser policy requires the user to be a valid user
///  - Both policies require the user to be authenticated
/// </summary>
public static class AuthorizationConfig
{
    public static void Configure(IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("IsAdmin", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.Requirements.Add(new IsAdminRequirement(UserRoles.Admin));
            });
            options.AddPolicy("IsUser", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new IsUserRequirement(UserRoles.User));
            });
        });
    }
}