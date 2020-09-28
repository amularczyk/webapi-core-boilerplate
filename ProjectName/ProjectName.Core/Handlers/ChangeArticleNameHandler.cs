using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ProjectName.Core.Interfaces.Repositories;

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

        public ChangeArticleNameHandler(
            ILogger<ChangeArticleNameHandler> logger,
            IArticlesRepository articlesRepository
            )
        {
            _logger = logger;
            _articlesRepository = articlesRepository;
        }

        protected override async Task Handle(ChangeArticleName request, CancellationToken cancellationToken)
        {
            var item = await _articlesRepository.RetrieveByIdAsync(request.ArticleId).ConfigureAwait(false);

            item.ChangeName(request.ArticleName);

            await _articlesRepository.UpdateAsync(item).ConfigureAwait(false);
        }
    }
}