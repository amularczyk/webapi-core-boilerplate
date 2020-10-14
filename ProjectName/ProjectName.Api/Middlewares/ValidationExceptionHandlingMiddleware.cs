using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProjectName.Core.Exceptions;

namespace ProjectName.Api.Middlewares
{
    public class ValidationExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(e.Message);
            }
        }
    }
}