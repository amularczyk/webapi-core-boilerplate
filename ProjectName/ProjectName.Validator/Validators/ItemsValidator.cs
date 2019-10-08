using System.Threading.Tasks;
using ProjectName.Core.Interfaces.Validators;
using ProjectName.Core.Models;

namespace ProjectName.Validator.Validators
{
    public class ItemsValidator : IItemsValidator
    {
        public async Task ValidateEntityAsync(Item entity)
        {
        }
    }
}