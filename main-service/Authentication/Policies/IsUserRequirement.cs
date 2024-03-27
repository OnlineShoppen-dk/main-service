using Microsoft.AspNetCore.Authorization;

namespace main_service.Authentication.Policies;

/// <summary>
/// This class is used to check if the user is a valid user
/// Admin is also considered a valid user
/// </summary>
public class IsUserRequirement : IAuthorizationRequirement
{
    public string RoleOfUser { get; }

    public IsUserRequirement(string roleOfUser)
    {
        RoleOfUser = roleOfUser;
    }
}

public class IsUserHandler : AuthorizationHandler<IsUserRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsUserRequirement requirement)
    {
        if (context.User.IsInRole(requirement.RoleOfUser) || context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}