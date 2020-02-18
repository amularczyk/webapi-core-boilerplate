using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace ProjectName.Validator
{
    public abstract class BaseValidator<T> : AbstractValidator<T>, Core.Interfaces.Validators.IValidator<T>
        where T : class
    {
        protected BaseValidator()
        {
            ValidatorOptions.LanguageManager.Enabled = false;
        }

        public virtual async Task<ValidationResult> ValidateEntityAsync(T entity)
        {
            return await ValidateAsync(entity);
        }

        public virtual async Task ValidateEntityAndThrowAsync(T entity)
        {
            await this.ValidateAndThrowAsync(entity);
        }
    }
}