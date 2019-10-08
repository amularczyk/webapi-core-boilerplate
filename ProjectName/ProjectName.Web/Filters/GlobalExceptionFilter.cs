using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProjectName.Web.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;

            switch (ex)
            {
                default:
                    context.Result = new BadRequestObjectResult(ex.Message);
                    break;
            }
        }
    }
}