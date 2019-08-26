using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectName.Core.Interfaces.Repositories;
using ProjectName.Core.Interfaces.Services;
using ProjectName.Core.Models;

namespace ProjectName.Core.Services
{
    public class ItemsReadService : IItemsReadService
    {
        private readonly IItemsRepository _itemsRepository;

        public ItemsReadService(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        public async Task<IEnumerable<Item>> RetrieveAllAsync()
        {
            return await _itemsRepository.RetrieveAllAsync();
        }

        public async Task<Item> RetrieveByIdAsync(Guid itemId)
        {
            return await _itemsRepository.RetrieveByIdAsync(itemId);
        }
    }
}