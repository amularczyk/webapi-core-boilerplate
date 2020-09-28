using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ProjectName.Core.Interfaces.Repositories;
using ProjectName.Core.Interfaces.Validators;

namespace ProjectName.Core.Handlers
{
    public class ChangeArticleName : IRequest
    {
        public ChangeArticleName(Guid articleId, string articleName)
        {
            ArticleId = articleId;
            ArticleName = articleName;
        }

        public Guid ArticleId { get; }
        public string ArticleName { get; }
    }

    public class ChangeArticleNameHandler : AsyncRequestHandler<ChangeArticleName>
    {
        private readonly ILogger<ChangeArticleNameHandler> _logger;
        private readonly IArticlesRepository _articlesRepository;
        private readonly IArticleValidator _articleValidator;

        public ChangeArticleNameHandler(
            ILogger<ChangeArticleNameHandler> logger,
            IArticlesRepository articlesRepository,
            IArticleValidator articleValidator
        )
        {
            _logger = logger;
            _articlesRepository = articlesRepository;
            _articleValidator = articleValidator;
        }

        protected override async Task Handle(ChangeArticleName request, CancellationToken cancellationToken)
        {
            var article = await _articlesRepository.RetrieveByIdAsync(request.ArticleId).ConfigureAwait(false);

            article.ChangeName(request.ArticleName);

            await _articleValidator.ValidateEntityAndThrowAsync(article).ConfigureAwait(false);

            await _articlesRepository.UpdateAsync(article).ConfigureAwait(false);
        }
    }
}