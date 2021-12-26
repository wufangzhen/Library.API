using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Common.ComplexAuthenticate
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeCodeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public AuthorizeCodeAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)

        {
            if (!context.HttpContext.User.HasClaim(c => c.Value == Name))
            {
                context.Result = new ForbidResult();

            }
            await Task.CompletedTask;

        }

    }

}
