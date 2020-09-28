using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ProjectName.Api.E2ETests.Models;
using ProjectName.Api.Models;
using Shouldly;
using Xunit;

namespace ProjectName.Api.E2ETests.Controllers
{
    [Collection(nameof(SharedDatabase))]
    public class ItemsControllerTests : ApiTestsBase
    {
        public ItemsControllerTests(WebApiTesterFactory factory)
        {
            _factory = factory;
        }

        private readonly WebApiTesterFactory _factory;

        [Fact]
        public async Task ChangeNameAsync_ShouldChangeItemName()
        {
            // arrange
            var item = new ItemViewModel { Name = Guid.NewGuid().ToString() };
            var newName = Guid.NewGuid().ToString();

            var client = _factory.CreateClient();

            var addItemResponse = await client.PostAsync(ItemsUrl, GetContent(item)).ConfigureAwait(false);
            var itemId = await GetResult<Guid>(addItemResponse).ConfigureAwait(false);

            // act
            var response = await client.PatchAsync($"{ItemsUrl}/{itemId}?name={newName}", null).ConfigureAwait(false);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            var changedItem = await client.GetAsync($"{ItemsUrl}/{itemId}").ConfigureAwait(false);
            var result = await GetResult<TestItem>(changedItem).ConfigureAwait(false);
            result.Name.ShouldBe(newName);
            result.Name.ShouldNotBe(item.Name);
        }

        [Fact]
        public async Task ChangeNameAsync_ShouldReturnError_WhenNameIsEmpty()
        {
            // arrange
            var item = new ItemViewModel { Name = Guid.NewGuid().ToString() };
            var newName = string.Empty;

            var client = _factory.CreateClient();

            var addItemResponse = await client.PostAsync(ItemsUrl, GetContent(item)).ConfigureAwait(false);
            var itemId = await GetResult<Guid>(addItemResponse).ConfigureAwait(false);

            // act
            var response = await client.PatchAsync($"{ItemsUrl}/{itemId}?name={newName}", null).ConfigureAwait(false);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            result.ShouldContain("Validation failed:");
            result.ShouldContain("Name: 'Name' must not be empty.");
        }

        [Fact]
        public async Task InsertAsync_ShouldAddedNewItem()
        {
            // arrange
            var item = new ItemViewModel { Name = Guid.NewGuid().ToString() };

            var client = _factory.CreateClient();

            // act
            var response = await client.PostAsync(ItemsUrl, GetContent(item)).ConfigureAwait(false);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var result = await GetResult<Guid>(response).ConfigureAwait(false);
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task InsertAsync_ShouldReturnError_WhenNameIsEmpty()
        {
            // arrange
            var item = new ItemViewModel();

            var client = _factory.CreateClient();

            // act
            var response = await client.PostAsync(ItemsUrl, GetContent(item)).ConfigureAwait(false);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            result.ShouldContain("Validation failed:");
            result.ShouldContain("Name: 'Name' must not be empty.");
        }

        [Fact]
        public async Task RetrieveAllAsync_ShouldReturnAddedItemInItemList()
        {
            // arrange
            var item = new ItemViewModel { Name = Guid.NewGuid().ToString() };

            var client = _factory.CreateClient();

            await client.PostAsync(ItemsUrl, GetContent(item)).ConfigureAwait(false);

            // act
            var response = await client.GetAsync(ItemsUrl).ConfigureAwait(false);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var result = await GetResult<IList<TestItem>>(response).ConfigureAwait(false);
            result.Count.ShouldBeGreaterThanOrEqualTo(1);
            result.FirstOrDefault(r => r.Name == item.Name).ShouldNotBeNull();
        }

        [Fact]
        public async Task RetrieveAllAsync_ShouldReturnAllItems()
        {
            // arrange
            var client = _factory.CreateClient();

            // act
            var response = await client.GetAsync(ItemsUrl).ConfigureAwait(false);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var result = await GetResult<IList<TestItem>>(response).ConfigureAwait(false);
            result.Count.ShouldBeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public async Task RetrieveByIdAsync_ShouldReturnAddedItemDetails()
        {
            // arrange
            var item = new ItemViewModel { Name = Guid.NewGuid().ToString() };

            var client = _factory.CreateClient();

            var addItemResponse = await client.PostAsync(ItemsUrl, GetContent(item)).ConfigureAwait(false);
            var itemId = await GetResult<Guid>(addItemResponse).ConfigureAwait(false);

            // act
            var response = await client.GetAsync($"{ItemsUrl}/{itemId}").ConfigureAwait(false);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var result = await GetResult<TestItem>(response).ConfigureAwait(false);
            result.Id.ShouldBe(itemId);
            result.Name.ShouldBe(item.Name);
        }
    }
}