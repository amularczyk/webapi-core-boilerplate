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
            return result.Status switch
            {
                OperationResultStatus.Success => (result.GetValue() != null
                    ? (IActionResult)Ok(result.GetValue())
                    : (IActionResult)NoContent()),
                OperationResultStatus.Forbid => new ObjectResult(result.Message)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                },
                OperationResultStatus.NotFound => NotFound(result.Message),
                OperationResultStatus.Error => BadRequest(result.Message),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}