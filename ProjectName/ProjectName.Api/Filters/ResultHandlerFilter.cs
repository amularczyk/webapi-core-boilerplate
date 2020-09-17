using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProjectName.Api
{
    public class ResultHandlerFilter : IAlwaysRunResultFilter // IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context != null &&
                context.Result is ObjectResult resultObj &&
                resultObj.Value is OperationResult result)
            {
                context.Result = result.Status switch
                {
                    OperationResultStatus.Success => (result.GetValue() != null
                        ? (IActionResult)new OkObjectResult(result.GetValue())
                        : (IActionResult)new NoContentResult()),
                    OperationResultStatus.Forbid => new ObjectResult(result.Message)
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    },
                    OperationResultStatus.NotFound => new NotFoundObjectResult(result.Message),
                    OperationResultStatus.Error => new BadRequestObjectResult(result.Message),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }

    public class OperationResult
    {
        public OperationResult(string message, OperationResultStatus status)
        {
            Message = message;
            Status = status;
        }

        public string Message { get; }
        public OperationResultStatus Status { get; }

        public virtual object GetValue()
        {
            return null;
        }

        public static OperationResult Success()
        {
            return new OperationResult(string.Empty, OperationResultStatus.Success);
        }

        public static OperationResult Error(string message)
        {
            return new OperationResult(message, OperationResultStatus.Error);
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public OperationResult(T value, string message, OperationResultStatus status)
            : base(message, status)
        {
            Value = value;
        }

        public T Value { get; }

        public override object GetValue()
        {
            return Value;
        }

        public static OperationResult<T> Success(T value)
        {
            return new OperationResult<T>(value, string.Empty,
                OperationResultStatus.Success);
        }

        public static OperationResult<T> Error(OperationResult operationResult)
        {
            return new OperationResult<T>(default, operationResult.Message,
                OperationResultStatus.Error);
        }
    }

    public enum OperationResultStatus
    {
        Success,
        Error,
        Forbid,
        NotFound
    }
}