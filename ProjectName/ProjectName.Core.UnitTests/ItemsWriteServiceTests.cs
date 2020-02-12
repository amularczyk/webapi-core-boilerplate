using System;
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
    public class ItemsWriteServiceTests
    {
        public ItemsWriteServiceTests()
        {
            _mocker = new AutoMoqer();
        }

        private readonly AutoMoqer _mocker;

        [Fact]
        public async Task RetrieveByIdAsync_ShouldReturnItemForGivenId()
        {
            // Arrange
            var item = new Item();

            var sut = _mocker.Create<ItemsWriteService>();

            var repositoryMock = _mocker.GetMock<IItemsRepository>();
            repositoryMock.Setup(s => s.InsertAsync(It.IsAny<Item>())).ReturnsAsync(true);

            // Act
            var result = await sut.InsertAsync(item);

            // Assert
            item.Id.ShouldNotBe(Guid.Empty);
            result.ShouldBe(item.Id);
            repositoryMock.Verify(v => v.InsertAsync(It.Is<Item>(p => p == item)), Times.Once);
        }
    }
}