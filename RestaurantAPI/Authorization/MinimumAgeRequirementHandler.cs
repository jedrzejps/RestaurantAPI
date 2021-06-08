using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateOfBirthClaim = context.User.FindFirst(c => c.Type == "DateOfBirth");
            if(dateOfBirthClaim is null)
            {
                return Task.CompletedTask;
            }
            var dateOfBirth = DateTime.Parse(dateOfBirthClaim.Value);
            if(dateOfBirth.AddDays(requirement.MinimumAge) <= DateTime.Today)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
