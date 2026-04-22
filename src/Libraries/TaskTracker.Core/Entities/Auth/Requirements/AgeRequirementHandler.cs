using Microsoft.AspNetCore.Authorization;

namespace TaskTracker.Core.Entities.Auth.Requirements
{
    public class AgeRequirementHandler : AuthorizationHandler<AgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AgeRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == "age" &&
                int.Parse(x.Value) > 10 && int.Parse(x.Value) < 50))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
