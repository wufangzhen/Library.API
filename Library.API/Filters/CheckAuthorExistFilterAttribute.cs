using Library.API.Entities;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Filters
{
    public class CheckAuthorExistFilterAttribute : ActionFilterAttribute
    {
        public CheckAuthorExistFilterAttribute(IRepositoryBase<Author, Guid> authorRepository)
        {
            AuthorRepository = authorRepository;
        }

        public IRepositoryBase<Author, Guid> AuthorRepository { get; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authorIdParameter = context.ActionArguments.Single(m => m.Key == "authorId");
            Guid authorId = (Guid)authorIdParameter.Value;

            var isExist = await AuthorRepository.IsExistAsync(authorId);
            if (!isExist)
            {
                context.Result = new NotFoundResult();
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}