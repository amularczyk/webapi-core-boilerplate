using System.Threading.Tasks;
using FluentValidation;

namespace ProjectName.Validator
{
    public abstract class BaseValidator<T> : AbstractValidator<T>, Core.Interfaces.Validators.IValidator<T>
        where T : class
    {
        protected BaseValidator()
        {
            ValidatorOptions.LanguageManager.Enabled = false;
        }

        public virtual async Task ValidateEntityAsync(T entity)
        {
            var result = await ValidateAsync(entity);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}