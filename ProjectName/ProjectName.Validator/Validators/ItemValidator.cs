using FluentValidation;
using ProjectName.Core.Interfaces.Validators;
using ProjectName.Core.Models;

namespace ProjectName.Validator.Validators
{
    public class ItemValidator : BaseValidator<Item>, IItemValidator
    {
        public ItemValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty();
        }
    }
}