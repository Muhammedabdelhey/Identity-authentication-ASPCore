using Microsoft.AspNetCore.Authorization;

namespace Identity_Authentication.Authorization
{
    public class AgeAuthorizationRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; }

        public AgeAuthorizationRequirement(int MinimumAge) => this.MinimumAge = MinimumAge;
    }
}
