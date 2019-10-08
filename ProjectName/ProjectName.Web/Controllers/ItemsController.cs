using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectName.Core.Interfaces.Services;
using ProjectName.Core.Models;

namespace ProjectName.Web.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsReadService _itemsReadService;
        private readonly IItemsWriteService _itemsWriteService;

        public ItemsController(IItemsReadService itemsReadService, IItemsWriteService itemsWriteService)
        {
            _itemsReadService = itemsReadService;
            _itemsWriteService = itemsWriteService;
        }

        [HttpGet]
        public async Task<IActionResult> RetrieveAllAsync()
        {
            var items = await _itemsReadService.RetrieveAllAsync();

            return new OkObjectResult(items);
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> RetrieveByIdAsync(Guid itemId)
        {
            var item = await _itemsReadService.RetrieveByIdAsync(itemId);

            return new OkObjectResult(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Item request)
        {
            var itemId = await _itemsWriteService.InsertAsync(request);

            return new OkObjectResult(itemId);
        }
    }
}