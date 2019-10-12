using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMoqCore;
using FluentValidation;
using ProjectName.Core.Models;
using ProjectName.Validator.Validators;
using Shouldly;
using Xunit;

namespace ProjectName.Validator.UnitTests
{
    public class ItemsValidatorTests
    {
        private readonly AutoMoqer _mocker;

        public ItemsValidatorTests()
        {
            _mocker = new AutoMoqer();
        }

        [Fact]
        public async Task ValidateEntityAsync_ShouldNotThrowAnyException_WhenModelIsValid()
        {
            // Arrange
            var item = GetValidItem();

            var sut = _mocker.Create<ItemsValidator>();

            // Act
            await sut.ValidateEntityAsync(item);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ValidateEntityAsync_ShouldThrowValidationException_WhenNameIsEmpty(string name)
        {
            // Arrange
            var item = GetValidItem();
            item.Name = name;

            var sut = _mocker.Create<ItemsValidator>();

            // Act
            var action = (Func<Task>)(() => sut.ValidateEntityAsync(item));

            // Assert
            var ex = await Should.ThrowAsync<ValidationException>(action);
            ex.Message.ShouldContain("Validation failed:");
            ex.Message.ShouldContain("Name: 'Name' must not be empty.");
        }

        [Fact]
        public async Task ValidateAsync_ShouldNotReturnAnyErrors_WhenModelIsValid()
        {
            // Arrange
            var item = GetValidItem();

            var sut = _mocker.Create<ItemsValidator>();

            // Act
            var result = await sut.ValidateAsync(item);

            // Assert
            result.IsValid.ShouldBeTrue();
            result.Errors.ShouldBeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ValidateAsync_ShouldReturnError_WhenNameIsEmpty(string name)
        {
            // Arrange
            var item = GetValidItem();
            item.Name = name;

            var sut = _mocker.Create<ItemsValidator>();

            // Act
            var result = await sut.ValidateAsync(item);

            // Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().PropertyName.ShouldBe(nameof(Item.Name));
            result.Errors.First().ErrorMessage.ShouldBe("'Name' must not be empty.");
        }

        private static Item GetValidItem()
        {
            return new Item { Name = Guid.NewGuid().ToString() };
        }
    }
}