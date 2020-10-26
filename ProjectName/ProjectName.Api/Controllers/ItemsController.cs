using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectName.Api.Models;
using ProjectName.Core.Exceptions;
using ProjectName.Core.Interfaces.Services;
using ProjectName.Core.Models;

namespace ProjectName.Api.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsReadService _itemsReadService;
        private readonly IItemsWriteService _itemsWriteService;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(
            ILogger<ItemsController> logger,
            IItemsReadService itemsReadService,
            IItemsWriteService itemsWriteService
        )
        {
            _logger = logger;
            _itemsReadService = itemsReadService;
            _itemsWriteService = itemsWriteService;
        }

        [HttpGet("tmp")]
        public async Task<OperationResult> Tmp()
        {
            return new OperationResult("is it working?", OperationResultStatus.Error);
            return OperationResult<OperationResult>.Success(new OperationResult("is it working?", OperationResultStatus.Error));
        }

        [HttpGet]
        public async Task<IActionResult> RetrieveAll()
        {
            var items = await _itemsReadService.RetrieveAllAsync().ConfigureAwait(false);

            _logger.LogInformation("Getting all items!");

            return Ok(items);
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> RetrieveById(Guid itemId)
        {
            var item = await _itemsReadService.RetrieveByIdAsync(itemId).ConfigureAwait(false);

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]ItemViewModel request)
        {
            var item = new Item(request.Name);
            var itemId = await _itemsWriteService.InsertAsync(item).ConfigureAwait(false);

            return Ok(itemId);
        }

        [HttpPatch("{itemId}")]
        public async Task<IActionResult> ChangeName(Guid itemId, string name)
        {
            await _itemsWriteService.ChangeNameAsync(itemId, name).ConfigureAwait(false);

            return NoContent();
        }
    }
}