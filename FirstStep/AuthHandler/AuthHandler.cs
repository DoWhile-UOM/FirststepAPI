using FirstStep.AuthHandler;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


public class AuthHandler : AuthorizationHandler<AuthRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthRequirement requirement)
    {

        var claim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claim != "1075")
        {
            Console.WriteLine(claim);
            context.Fail();

        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}