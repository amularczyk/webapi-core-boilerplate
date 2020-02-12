using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectName.Core.Interfaces;

namespace ProjectName.Api.Controllers.Res
{
    public enum OperationResultStatus
    {
        Success,
        Error
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

        public static OperationResult<T> Success(T value)
        {
            return new OperationResult<T>(value, string.Empty, OperationResultStatus.Success);
        }

        public static OperationResult<T> Error(OperationResult operationResult)
        {
            return new OperationResult<T>(default, operationResult.Message, OperationResultStatus.Error);
        }
    }

    [Route("api/SomeObjectRes")]
    [ApiController]
    public class SomeObjectController : ControllerBase
    {
        private readonly SomeObjectManager _someObjectManager;

        public SomeObjectController(SomeObjectManager someObjectManager)
        {
            _someObjectManager = someObjectManager;
        }

        [HttpGet("{value}")]
        public async Task<IActionResult> Execute(int value)
        {
            var doSomethingResult = await _someObjectManager.DoSomething(value);

            if (doSomethingResult.Status != OperationResultStatus.Error)
            {
                return new OkObjectResult(doSomethingResult.Value);
            }

            return new BadRequestObjectResult(doSomethingResult.Message);
        }
    }

    public class SomeObjectManager
    {
        private readonly ISomeObject1Service _someObject1Service;
        private readonly ISomeObject2Service _someObject2Service;
        private readonly ISomeObject3Service _someObject3Service;

        public SomeObjectManager(
            ISomeObject1Service someObject1Service,
            ISomeObject2Service someObject2Service,
            ISomeObject3Service someObject3Service
        )
        {
            _someObject1Service = someObject1Service;
            _someObject2Service = someObject2Service;
            _someObject3Service = someObject3Service;
        }

        public async Task<OperationResult<bool>> DoSomething(int value)
        {
            var doSomeWorkResult = await _someObject1Service.DoSomeWork(value);
            if (doSomeWorkResult.Status == OperationResultStatus.Error)
            {
                return OperationResult<bool>.Error(doSomeWorkResult);
            }

            var doSomeMoreWorkResult = await _someObject2Service.DoSomeMoreWork(value);
            if (doSomeMoreWorkResult.Status == OperationResultStatus.Error)
            {
                return OperationResult<bool>.Error(doSomeMoreWorkResult);
            }

            var doSomeOtherWorkResult = await _someObject3Service.DoSomeOtherWork(value);
            if (doSomeOtherWorkResult.Status == OperationResultStatus.Error)
            {
                return OperationResult<bool>.Error(doSomeOtherWorkResult);
            }

            return OperationResult<bool>.Success(true);
        }
    }

    public interface ISomeObject1Service : ITransient
    {
        Task<OperationResult> DoSomeWork(int value);
    }

    public interface ISomeObject2Service : ITransient
    {
        Task<OperationResult> DoSomeMoreWork(int value);
    }

    public interface ISomeObject3Service : ITransient
    {
        Task<OperationResult> DoSomeOtherWork(int value);
    }

    public class SomeObject1Service : ISomeObject1Service
    {
        public async Task<OperationResult> DoSomeWork(int value)
        {
            if (value > 10)
                // ...
            {
                return OperationResult.Success();
            }

            return OperationResult.Error("Operation cannot be processed.");
        }
    }

    public class SomeObject2Service : ISomeObject2Service
    {
        public async Task<OperationResult> DoSomeMoreWork(int value)
        {
            if (value > 5)
                // ...
            {
                return OperationResult.Success();
            }

            return OperationResult.Error("Operation cannot be processed.");
        }
    }

    public class SomeObject3Service : ISomeObject3Service
    {
        public async Task<OperationResult> DoSomeOtherWork(int value)
        {
            if (value > 20)
                // ...
            {
                return OperationResult.Success();
            }

            return OperationResult.Error("Operation cannot be processed.");
        }
    }
}