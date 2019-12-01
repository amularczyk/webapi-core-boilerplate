using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ProjectName.Web.Filters
{
    public class RequestTimeLoggingMiddleware
    {
        private static ILogger<RequestTimeLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var start = Stopwatch.GetTimestamp();
            await _next(httpContext);
            var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

            var statusCode = httpContext.Response?.StatusCode;
            var level = statusCode > 499 ? LogLevel.Error : LogLevel.Information;

            _logger.Log(level, $"Request processing time: {elapsedMs:0.0000} ms");
        }

        private static double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
    }
}