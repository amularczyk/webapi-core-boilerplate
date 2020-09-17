using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectName.Core.Exceptions;

namespace ProjectName.Api.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            context.Result = context.Exception switch
            {
                ValidationException ex => new BadRequestObjectResult(ex.Message),
                NoFoundException ex => new NotFoundObjectResult(ex.Message),
                AuthorizationException ex => new ObjectResult(ex.Message)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                },
                _ => new BadRequestObjectResult(context.Exception.Message)
            };
        }
    }
}