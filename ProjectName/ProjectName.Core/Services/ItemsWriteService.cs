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
            await _itemValidator.ValidateEntityAndThrowAsync(item).ConfigureAwait(false);

            await _itemsRepository.InsertAsync(item).ConfigureAwait(false);

            return item.Id;
        }

        public async Task ChangeNameAsync(Guid itemId, string name)
        {
            var item = await _itemsRepository.RetrieveByIdAsync(itemId).ConfigureAwait(false);

            item.ChangeName(name);

            await _itemValidator.ValidateEntityAndThrowAsync(item).ConfigureAwait(false);

            await _itemsRepository.UpdateAsync(item).ConfigureAwait(false);
        }
    }
}