using FluentValidation;
using ProjectName.Core.Interfaces.Validators;
using ProjectName.Core.Models;

namespace ProjectName.Validator.Validators
{
    public class ItemsValidator : BaseValidator<Item>, IItemsValidator
    {
        public ItemsValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty();
        }
    }
}