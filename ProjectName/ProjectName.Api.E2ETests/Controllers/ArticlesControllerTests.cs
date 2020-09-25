using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ProjectName.Api.E2ETests.Models;
using ProjectName.Api.Models;
using ProjectName.Core.Models;
using Shouldly;
using Xunit;

namespace ProjectName.Api.E2ETests.Controllers
{
    [Collection(nameof(SharedDatabase))]
    public class ArticlesControllerTests : ApiTestsBase
    {
        public ArticlesControllerTests(WebApiTesterFactory factory)
        {
            _factory = factory;
        }

        private readonly WebApiTesterFactory _factory;

        [Fact]
        public async Task InsertAsync_ShouldAddedNewArticle()
        {
            // arrange
            var article = new ArticleViewModel { Name = Guid.NewGuid().ToString()};

            var client = _factory.CreateClient();

            // act
            var response = await client.PostAsync(ArticlesUrl, GetContent(article)).ConfigureAwait(false);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var result = await GetResult<Guid>(response).ConfigureAwait(false);
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task InsertAsync_ShouldReturnError_WhenNameIsEmpty()
        {
            // arrange
            var article = new ArticleViewModel();

            var client = _factory.CreateClient();

            // act
            var response = await client.PostAsync(ArticlesUrl, GetContent(article)).ConfigureAwait(false);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            result.ShouldContain("Validation failed:");
            result.ShouldContain("Name: 'Name' must not be empty.");
        }

        [Fact]
        public async Task RetrieveAllAsync_ShouldReturnAddedArticleInArticleList()
        {
            // arrange
            var article = new ArticleViewModel { Name = Guid.NewGuid().ToString()};

            var client = _factory.CreateClient();

            await client.PostAsync(ArticlesUrl, GetContent(article)).ConfigureAwait(false);

            // act
            var response = await client.GetAsync(ArticlesUrl).ConfigureAwait(false);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var result = await GetResult<IList<TestArticle>>(response).ConfigureAwait(false);
            result.Count.ShouldBeGreaterThanOrEqualTo(1);
            result.FirstOrDefault(r => r.Name == article.Name).ShouldNotBeNull();
        }

        [Fact]
        public async Task RetrieveAllAsync_ShouldReturnAllArticles()
        {
            // arrange
            var client = _factory.CreateClient();

            // act
            var response = await client.GetAsync(ArticlesUrl).ConfigureAwait(false);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var result = await GetResult<IList<TestArticle>>(response).ConfigureAwait(false);
            result.Count.ShouldBeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public async Task RetrieveByIdAsync_ShouldReturnAddedArticleDetails()
        {
            // arrange
            var article = new ArticleViewModel { Name = Guid.NewGuid().ToString()};

            var client = _factory.CreateClient();

            var addArticleResponse = await client.PostAsync(ArticlesUrl, GetContent(article)).ConfigureAwait(false);
            var ArticleId = await GetResult<Guid>(addArticleResponse).ConfigureAwait(false);

            // act
            var response = await client.GetAsync($"{ArticlesUrl}/{ArticleId}").ConfigureAwait(false);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var result = await GetResult<TestArticle>(response).ConfigureAwait(false);
            result.Id.ShouldBe(ArticleId);
            result.Name.ShouldBe(article.Name);
        }
    }
}