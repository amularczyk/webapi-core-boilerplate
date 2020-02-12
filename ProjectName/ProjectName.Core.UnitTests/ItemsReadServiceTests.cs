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

namespace ProjectName.Core.UnitTests
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
            var items = new[] {new Item()};

            var sut = _mocker.Create<ItemsReadService>();

            var repositoryMock = _mocker.GetMock<IItemsRepository>();
            repositoryMock.Setup(s => s.RetrieveAllAsync()).ReturnsAsync(items);

            // Act
            var result = (await sut.RetrieveAllAsync()).ToList();

            // Assert
            result.Count.ShouldBe(items.Length);
            result[0].ShouldBe(items[0]);
            repositoryMock.Verify(v => v.RetrieveAllAsync(), Times.Once);
        }

        [Fact]
        public async Task RetrieveByIdAsync_ShouldReturnItemForGivenId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new Item {Id = id};

            var sut = _mocker.Create<ItemsReadService>();

            var repositoryMock = _mocker.GetMock<IItemsRepository>();
            repositoryMock.Setup(s => s.RetrieveByIdAsync(It.IsAny<Guid>())).ReturnsAsync(item);

            // Act
            var result = await sut.RetrieveByIdAsync(id);

            // Assert
            result.ShouldBe(item);
            repositoryMock.Verify(v => v.RetrieveByIdAsync(It.Is<Guid>(p => p == id)), Times.Once);
        }
    }
}