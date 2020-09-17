using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectName.Core.Exceptions;

namespace ProjectName.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected async Task<IActionResult> HandleException(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action().ConfigureAwait(false);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (NoFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (AuthorizationException e)
            {
                return new ObjectResult(e.Message)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }

        protected IActionResult HandleOperationResult(OperationResult result)
        {
            switch (result.Status)
            {
                case OperationResultStatus.Success:
                    if (result.GetValue() != null)
                        return Ok(result.GetValue());
                    return NoContent();
                case OperationResultStatus.Forbid:
                    return new ObjectResult(result.Message)
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                case OperationResultStatus.NotFound:
                    return NotFound(result.Message);
                case OperationResultStatus.Error:
                    return BadRequest(result.Message);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}