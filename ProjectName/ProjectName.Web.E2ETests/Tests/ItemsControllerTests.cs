using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ProjectName.Core.Models;
using Shouldly;
using Xunit;

namespace ProjectName.Web.E2ETests.Tests
{
    [Collection("DatabaseCollection")]
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
            result.Count.ShouldBe(0);
        }
    }
}