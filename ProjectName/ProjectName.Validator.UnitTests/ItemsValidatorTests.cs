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
        public ItemsValidatorTests()
        {
            _mocker = new AutoMoqer();
        }

        private readonly AutoMoqer _mocker;

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ValidateEntityAsync_ShouldThrowValidationException_WhenNameIsEmpty(string name)
        {
            // Arrange
            var item = GetValidItem(name);

            var sut = _mocker.Create<ItemValidator>();

            // Act
            var action = (Func<Task>) (() => sut.ValidateAndThrowAsync(item));

            // Assert
            var ex = await Should.ThrowAsync<ValidationException>(action).ConfigureAwait(false);
            ex.Message.ShouldContain("Validation failed:");
            ex.Message.ShouldContain("Name: 'Name' must not be empty.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ValidateAsync_ShouldReturnError_WhenNameIsEmpty(string name)
        {
            // Arrange
            var item = GetValidItem(name);

            var sut = _mocker.Create<ItemValidator>();

            // Act
            var result = await sut.ValidateAsync(item).ConfigureAwait(false);

            // Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().PropertyName.ShouldBe(nameof(Item.Name));
            result.Errors.First().ErrorMessage.ShouldBe("'Name' must not be empty.");
        }

        private static Item GetValidItem(string name)
        {
            return new Item(name);
        }

        [Fact]
        public async Task ValidateAsync_ShouldNotReturnAnyErrors_WhenModelIsValid()
        {
            // Arrange
            var item = GetValidItem(Guid.NewGuid().ToString());

            var sut = _mocker.Create<ItemValidator>();

            // Act
            var result = await sut.ValidateAsync(item).ConfigureAwait(false);

            // Assert
            result.IsValid.ShouldBeTrue();
            result.Errors.ShouldBeEmpty();
        }

        [Fact]
        public async Task ValidateEntityAsync_ShouldNotThrowAnyException_WhenModelIsValid()
        {
            // Arrange
            var item = GetValidItem(Guid.NewGuid().ToString());

            var sut = _mocker.Create<ItemValidator>();

            // Act
            await sut.ValidateAndThrowAsync(item).ConfigureAwait(false);
        }
    }
}