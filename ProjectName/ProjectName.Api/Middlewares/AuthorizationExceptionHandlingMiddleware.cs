using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProjectName.Core.Exceptions;

namespace ProjectName.Api.Middlewares
{
    public class AuthorizationExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AuthorizationException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(e.Message);
            }
        }
    }
}