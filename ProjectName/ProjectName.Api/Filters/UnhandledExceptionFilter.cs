using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ProjectName.Api.Filters
{
    public class UnhandledExceptionFilter : IAsyncExceptionFilter, IOrderedFilter
    {
        private readonly ILogger<UnhandledExceptionFilter> _logger;

        public UnhandledExceptionFilter(ILogger<UnhandledExceptionFilter> logger)
        {
            _logger = logger;
        }

        public int Order { get; }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled)
            {
                return;
            }

            _logger.LogError(context.Exception, "Unhandled exception occured");

            context.Result = new BadRequestObjectResult(context.Exception.Message);
            context.ExceptionHandled = true;
        }

    }
}