using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ProjectName.Api.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IAuthorizationService authorizationService)
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = authorizationHeader != null && authorizationHeader.StartsWith("Bearer ")
                ? new string(authorizationHeader.Skip(7).ToArray()) 
                : string.Empty;

            if (string.IsNullOrWhiteSpace(token))
            {
                await httpContext.ChallengeAsync();
                return;
            }

            await _next(httpContext);
        }
    }
}