using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectName.Core.Interfaces.Services;
using ProjectName.Core.Models;

namespace ProjectName.Web.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly IItemsReadService _itemsReadService;
        private readonly IItemsWriteService _itemsWriteService;

        public ItemsController(ILogger<ItemsController> logger, IItemsReadService itemsReadService, IItemsWriteService itemsWriteService)
        {
            _logger = logger;
            _itemsReadService = itemsReadService;
            _itemsWriteService = itemsWriteService;
        }

        [HttpGet]
        public async Task<IActionResult> RetrieveAllAsync()
        {
            var items = await _itemsReadService.RetrieveAllAsync();

            _logger.LogInformation("Getting all items!");

            return new OkObjectResult(items);
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> RetrieveByIdAsync(Guid itemId)
        {
            var item = await _itemsReadService.RetrieveByIdAsync(itemId);

            return new OkObjectResult(item);
        }

        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]Item request)
        {
            var itemId = await _itemsWriteService.InsertAsync(request);

            return new OkObjectResult(itemId);
        }
    }
}