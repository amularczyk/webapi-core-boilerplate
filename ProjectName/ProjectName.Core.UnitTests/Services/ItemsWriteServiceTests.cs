using System;
using System.Threading.Tasks;
using AutoMoqCore;
using Moq;
using ProjectName.Core.Interfaces.Repositories;
using ProjectName.Core.Interfaces.Validators;
using ProjectName.Core.Models;
using ProjectName.Core.Services;
using Xunit;

namespace ProjectName.Core.UnitTests.Services
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
            var item = new Item(Guid.NewGuid().ToString());

            var sut = _mocker.Create<ItemsWriteService>();

            var validatorMock = _mocker.GetMock<IItemValidator>();

            var repositoryMock = _mocker.GetMock<IItemsRepository>();
            repositoryMock.Setup(s => s.InsertAsync(It.IsAny<Item>())).ReturnsAsync(true);

            // Act
            var result = await sut.InsertAsync(item).ConfigureAwait(false);

            // Assert
            validatorMock.Verify(v => v.ValidateEntityAndThrowAsync(It.Is<Item>(p => p == item)), Times.Once);
            repositoryMock.Verify(v => v.InsertAsync(It.Is<Item>(p => p == item)), Times.Once);
        }
    }
}