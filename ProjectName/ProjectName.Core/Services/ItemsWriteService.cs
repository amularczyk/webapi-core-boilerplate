using System;
using System.Threading.Tasks;
using ProjectName.Core.Interfaces.Repositories;
using ProjectName.Core.Interfaces.Services;
using ProjectName.Core.Interfaces.Validators;
using ProjectName.Core.Models;

namespace ProjectName.Core.Services
{
    public class ItemsWriteService : IItemsWriteService
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly IItemValidator _itemValidator;

        public ItemsWriteService(
            IItemsRepository itemsRepository,
            IItemValidator itemValidator
        )
        {
            _itemsRepository = itemsRepository;
            _itemValidator = itemValidator;
        }

        public async Task<Guid> InsertAsync(Item item)
        {
            await _itemValidator.ValidateEntityAsync(item);

            item.Id = Guid.NewGuid();

            await _itemsRepository.InsertAsync(item);

            return item.Id;
        }
    }
}