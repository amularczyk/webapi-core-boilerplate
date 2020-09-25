using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ProjectName.Core.Interfaces.Repositories;
using ProjectName.Core.Models;

namespace ProjectName.Core.Handlers
{
    public class GetArticle : IRequest<Article>
    {
        public Guid ArticleId { get; }

        public GetArticle(Guid articleId)
        {
            ArticleId = articleId;
        }
    }

    public class GetArticleHandler : IRequestHandler<GetArticle, Article>
    {
        private readonly ILogger<GetArticleHandler> _logger;
        private readonly IArticlesRepository _articlesRepository;

        public GetArticleHandler(
            ILogger<GetArticleHandler> logger,
            IArticlesRepository articlesRepository
            )
        {
            _logger = logger;
            _articlesRepository = articlesRepository;
        }

        public async Task<Article> Handle(GetArticle request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Logging test");
            return await _articlesRepository.RetrieveByIdAsync(request.ArticleId).ConfigureAwait(false);
        }
    }
}