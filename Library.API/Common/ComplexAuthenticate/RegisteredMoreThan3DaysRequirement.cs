using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Common.ComplexAuthenticate
{
    public class RegisteredMoreThan3DaysRequirement :
    AuthorizationHandler<RegisteredMoreThan3DaysRequirement>,
    IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RegisteredMoreThan3DaysRequirement requirement)
        {
            if (!context.User.HasClaim(cliam => cliam.Type == "RegisterDate"))
            {
                return Task.CompletedTask;
            }
            var regDate = Convert.ToDateTime(
                context.User.FindFirst(c => c.Type == "RegisterDate").Value);
            var timeSpan = DateTime.Now - regDate;
            if (timeSpan.TotalDays > 3)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

}
