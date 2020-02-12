using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectName.Core.Interfaces;

namespace ProjectName.Api.Controllers.Ex
{
    public class MyException : Exception
    {
        public MyException() : base("Operation cannot be processed.")
        {
        }
    }

    [Route("api/SomeObjectEx")]
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
            try
            {
                var item = await _someObjectManager.DoSomething(value);

                return new OkObjectResult(item);
            }
            catch (MyException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
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

        public async Task<bool> DoSomething(int value)
        {
            await _someObject1Service.DoSomeWork(value);
            await _someObject2Service.DoSomeMoreWork(value);
            await _someObject3Service.DoSomeOtherWork(value);

            return true;
        }
    }

    public interface ISomeObject1Service : ITransient
    {
        Task DoSomeWork(int value);
    }

    public interface ISomeObject2Service : ITransient
    {
        Task DoSomeMoreWork(int value);
    }

    public interface ISomeObject3Service : ITransient
    {
        Task DoSomeOtherWork(int value);
    }

    public class SomeObject1Service : ISomeObject1Service
    {
        public async Task DoSomeWork(int value)
        {
            if (value > 10)
            {
                // ...
                return;
            }

            throw new MyException();
        }
    }

    public class SomeObject2Service : ISomeObject2Service
    {
        public async Task DoSomeMoreWork(int value)
        {
            if (value > 5)
            {
                // ...
                return;
            }

            throw new MyException();
        }
    }

    public class SomeObject3Service : ISomeObject3Service
    {
        public async Task DoSomeOtherWork(int value)
        {
            if (value > 20)
            {
                // ...
                return;
            }

            throw new MyException();
        }
    }
}