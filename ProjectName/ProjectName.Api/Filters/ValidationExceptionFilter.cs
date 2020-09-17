using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectName.Core.Exceptions;

namespace ProjectName.Api.Filters
{
    public class ValidationExceptionFilter : IAsyncExceptionFilter, IOrderedFilter
    {
        public int Order { get; }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!(context.Exception is ValidationException exception))
                return;

            context.Result = new BadRequestObjectResult(exception.Message);
            context.ExceptionHandled = true;
        }
    }
    public class NoFoundExceptionFilter : IAsyncExceptionFilter, IOrderedFilter
    {
        public int Order { get; }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!(context.Exception is NoFoundException exception))
                return;

            context.Result = new BadRequestObjectResult(exception.Message);
            context.ExceptionHandled = true;
        }
    }
    public class AuthorizationExceptionFilter : IAsyncExceptionFilter, IOrderedFilter
    {
        public int Order { get; }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!(context.Exception is AuthorizationException exception))
                return;

            context.Result = new ObjectResult(exception.Message)
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            context.ExceptionHandled = true;
        }
    }
}