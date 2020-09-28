using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ProjectName.Core.Interfaces.Repositories;
using ProjectName.Core.Interfaces.Validators;
using ProjectName.Core.Models;

namespace ProjectName.Core.Handlers
{
    public class AddArticle : IRequest<Guid>
    {
        public AddArticle(Article article)
        {
            Article = article;
        }

        public Article Article { get; }
    }

    public class AddArticleHandler : IRequestHandler<AddArticle, Guid>
    {
        private readonly IArticlesRepository _articlesRepository;
        private readonly IArticleValidator _articleValidator;
        private readonly ILogger<AddArticleHandler> _logger;

        public AddArticleHandler(
            ILogger<AddArticleHandler> logger,
            IArticlesRepository articlesRepository,
            IArticleValidator articleValidator
        )
        {
            _logger = logger;
            _articlesRepository = articlesRepository;
            _articleValidator = articleValidator;
        }

        public async Task<Guid> Handle(AddArticle request, CancellationToken cancellationToken)
        {
            await _articleValidator.ValidateEntityAndThrowAsync(request.Article).ConfigureAwait(false);

            await _articlesRepository.InsertAsync(request.Article).ConfigureAwait(false);

            return request.Article.Id;
        }
    }
}