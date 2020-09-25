using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ProjectName.Core.Interfaces.Repositories;
using ProjectName.Core.Models;

namespace ProjectName.Core.Handlers
{
    public class AddArticle : IRequest<Guid>
    {
        public Article Article { get; }

        public AddArticle(Article article)
        {
            Article = article;
        }
    }

    public class AddArticleHandler : IRequestHandler<AddArticle, Guid>
    {
        private readonly ILogger<AddArticleHandler> _logger;
        private readonly IArticlesRepository _articlesRepository;

        public AddArticleHandler(
            ILogger<AddArticleHandler> logger,
            IArticlesRepository articlesRepository
            )
        {
            _logger = logger;
            _articlesRepository = articlesRepository;
        }

        public async Task<Guid> Handle(AddArticle request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Logging test");
            await _articlesRepository.InsertAsync(request.Article).ConfigureAwait(false);
            return request.Article.Id;
        }
    }
}