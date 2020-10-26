using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ProjectName.Api.Middlewares
{
    public class OperationResultHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public OperationResultHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var existingBody = context.Response.Body;

            using (var newBody = new MemoryStream())
            {
                context.Response.Body = newBody;

                await _next(context);

                newBody.Seek(0, SeekOrigin.Begin);
                var body = new StreamReader(newBody).ReadToEnd();

                context.Response.Body = existingBody;

                await TryHandleOperationResultAsync(context, body);
            }
        }

        private static async Task TryHandleOperationResultAsync(HttpContext context, string body)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<OperationResult<object>>(body);
                var bodyToReturn = result.GetValue() != null
                    ? JsonConvert.SerializeObject(result.GetValue())
                    : result.Message;

                switch (result.Status)
                {
                    case OperationResultStatus.Success:
                        var value = result.GetValue();
                        if (value != null)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                            await context.Response.WriteAsync(bodyToReturn);
                        }
                        else
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                        }

                        break;
                    case OperationResultStatus.Error:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "text/plain";
                        await context.Response.WriteAsync(bodyToReturn);
                        break;
                    case OperationResultStatus.Forbid:
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.ContentType = "text/plain";
                        await context.Response.WriteAsync(bodyToReturn);
                        break;
                    case OperationResultStatus.NotFound:
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        context.Response.ContentType = "text/plain";
                        await context.Response.WriteAsync(bodyToReturn);
                        break;
                }
            }
            catch (JsonSerializationException e)
            {
                // ...
                await context.Response.WriteAsync(body);
            }
        }
    }
}