using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Identity_Authentication.Authorization
{
    public class AgeAuthorizationHandler : AuthorizationHandler<AgeAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeAuthorizationRequirement requirement)
        {
            var BirthDateClaim = context.User.FindFirstValue("DateOfBirth");
            if (BirthDateClaim != null)
            {
                if (DateTime.Parse(BirthDateClaim).Year - DateTime.Now.Year >= requirement.MinimumAge)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
