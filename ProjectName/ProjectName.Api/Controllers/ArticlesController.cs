using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectName.Api.Models;
using ProjectName.Core.Handlers;
using ProjectName.Core.Models;

namespace ProjectName.Api.Controllers
{
    [ApiController]
    [Route("api/articles")]
    public class ArticlesController : ControllerBase
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly IMediator _mediator;

        public ArticlesController(
            ILogger<ItemsController> logger,
            IMediator mediator
        )
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> RetrieveAll()
        {
            var articles = await _mediator.Send(new GetAllArticles()).ConfigureAwait(false);

            _logger.LogInformation("Getting all items!");

            return Ok(articles);
        }

        [HttpGet("{articleId}")]
        public async Task<IActionResult> RetrieveById(Guid articleId)
        {
            var article = await _mediator.Send(new GetArticle(articleId)).ConfigureAwait(false);

            return Ok(article);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]ArticleViewModel request)
        {
            var article = new Article(request.Name);
            var articleId = await _mediator.Send(new AddArticle(article)).ConfigureAwait(false);

            return Ok(articleId);
        }

        //[HttpPatch("{itemId}")]
        //public async Task<IActionResult> ChangeName(Guid itemId, string name)
        //{
        //    await _itemsWriteService.ChangeNameAsync(itemId, name).ConfigureAwait(false);

        //    return NoContent();
        //}
    }
}