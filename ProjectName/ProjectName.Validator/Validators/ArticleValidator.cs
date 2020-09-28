using FluentValidation;
using ProjectName.Core.Interfaces.Validators;
using ProjectName.Core.Models;

namespace ProjectName.Validator.Validators
{
    public class ArticleValidator : BaseValidator<Article>, IArticleValidator
    {
        public ArticleValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty();
        }
    }
}