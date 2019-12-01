using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ProjectName.Core.Interfaces.Repositories;
using ProjectName.Core.Interfaces.Services;
using ProjectName.Core.Models;

namespace ProjectName.Core.Services
{
    public class ItemsReadService : IItemsReadService
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly ILogger<ItemsReadService> _logger;

        public ItemsReadService(IItemsRepository itemsRepository,
            ILogger<ItemsReadService> logger)
        {
            _itemsRepository = itemsRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Item>> RetrieveAllAsync()
        {
            _logger.LogInformation("Logging test");
            return await _itemsRepository.RetrieveAllAsync();
        }

        public async Task<Item> RetrieveByIdAsync(Guid itemId)
        {
            return await _itemsRepository.RetrieveByIdAsync(itemId);
        }
    }
}