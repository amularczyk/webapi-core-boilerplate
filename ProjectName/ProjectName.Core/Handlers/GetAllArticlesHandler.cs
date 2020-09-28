using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ProjectName.Core.Interfaces.Repositories;
using ProjectName.Core.Models;

namespace ProjectName.Core.Handlers
{
    public class GetAllArticles : IRequest<IEnumerable<Article>>
    {
    }

    public class GetAllArticlesHandler : IRequestHandler<GetAllArticles, IEnumerable<Article>>
    {
        private readonly IArticlesRepository _articlesRepository;
        private readonly ILogger<GetAllArticlesHandler> _logger;

        public GetAllArticlesHandler(
            ILogger<GetAllArticlesHandler> logger,
            IArticlesRepository articlesRepository
        )
        {
            _logger = logger;
            _articlesRepository = articlesRepository;
        }

        public async Task<IEnumerable<Article>> Handle(GetAllArticles request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Logging test");
            return await _articlesRepository.RetrieveAllAsync().ConfigureAwait(false);
        }
    }
}