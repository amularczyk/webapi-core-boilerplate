using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMoqCore;
using Moq;
using ProjectName.Core.Interfaces.Repositories;
using ProjectName.Core.Models;
using ProjectName.Core.Services;
using Shouldly;
using Xunit;

namespace ProjectName.Core.UnitTests.Services
{
    public class ItemsReadServiceTests
    {
        public ItemsReadServiceTests()
        {
            _mocker = new AutoMoqer();
        }

        private readonly AutoMoqer _mocker;

        [Fact]
        public async Task RetrieveAllAsync_ShouldReturnAllItems()
        {
            // Arrange
            var items = new[] { new Item(Guid.NewGuid().ToString()) };

            var sut = _mocker.Create<ItemsReadService>();

            var repositoryMock = _mocker.GetMock<IItemsRepository>();
            repositoryMock.Setup(s => s.RetrieveAllAsync()).ReturnsAsync(items);

            // Act
            var result = (await sut.RetrieveAllAsync().ConfigureAwait(false)).ToList();

            // Assert
            repositoryMock.Verify(v => v.RetrieveAllAsync(), Times.Once);
            result.Count.ShouldBe(items.Length);
            result[0].ShouldBe(items[0]);
        }

        [Fact]
        public async Task RetrieveByIdAsync_ShouldReturnItemForGivenId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new Item(Guid.NewGuid().ToString());

            var sut = _mocker.Create<ItemsReadService>();

            var repositoryMock = _mocker.GetMock<IItemsRepository>();
            repositoryMock.Setup(s => s.RetrieveByIdAsync(It.Is<Guid>(p => p == id))).ReturnsAsync(item);

            // Act
            var result = await sut.RetrieveByIdAsync(id).ConfigureAwait(false);

            // Assert
            repositoryMock.Verify(v => v.RetrieveByIdAsync(It.IsAny<Guid>()), Times.Once);
            result.ShouldBe(item);
        }
    }
}