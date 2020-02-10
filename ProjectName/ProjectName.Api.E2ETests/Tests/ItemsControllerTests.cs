using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ProjectName.Core.Models;
using Shouldly;
using Xunit;

namespace ProjectName.Api.E2ETests.Tests
{
    [Collection(nameof(SharedDatabase))]
    public class ItemsControllerTests : ApiTestsBase
    {
        private readonly WebApiTesterFactory _factory;

        public ItemsControllerTests(WebApiTesterFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task RetrieveAllAsync_ShouldReturnAllItems()
        {
            // arrange
            var client = _factory.CreateClient();

            // act
            var response = await client.GetAsync(ItemsUrl);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var result = await GetResult<IList<Item>>(response);
            result.Count.ShouldBeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public async Task RetrieveAllAsync_ShouldReturnAddedItemInItemList()
        {
            // arrange
            var item = new Item { Name = Guid.NewGuid().ToString() };

            var client = _factory.CreateClient();

            await client.PostAsync(ItemsUrl, GetContent(item));

            // act
            var response = await client.GetAsync(ItemsUrl);

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var result = await GetResult<IList<Item>>(response);
            result.Count.ShouldBeGreaterThanOrEqualTo(1);
            result.FirstOrDefault(r => r.Name == item.Name).ShouldNotBeNull();
        }

        [Fact]
        public async Task InsertAsync_ShouldAddedNewItem()
        {
            // arrange
            var item = new Item { Name = Guid.NewGuid().ToString() };

            var client = _factory.CreateClient();

            // act
            var response = await client.PostAsync(ItemsUrl, GetContent(item));

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var result = await GetResult<Guid>(response);
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task InsertAsync_ShouldReturnError_WhenNameIsEmpty()
        {
            // arrange
            var item = new Item();

            var client = _factory.CreateClient();

            // act
            var response = await client.PostAsync(ItemsUrl, GetContent(item));

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadAsStringAsync();
            result.ShouldContain("Validation failed:");
            result.ShouldContain("Name: 'Name' must not be empty.");
        }

        [Fact]
        public async Task RetrieveByIdAsync_ShouldReturnAddedItemDetails()
        {
            // arrange
            var item = new Item { Name = Guid.NewGuid().ToString() };

            var client = _factory.CreateClient();

            var addItemResponse = await client.PostAsync(ItemsUrl, GetContent(item));
            var itemId = await GetResult<Guid>(addItemResponse);

            // act
            var response = await client.GetAsync($"{ItemsUrl}/{itemId}");

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var result = await GetResult<Item>(response);
            result.Id.ShouldBe(itemId);
            result.Name.ShouldBe(item.Name);
        }
    }
}