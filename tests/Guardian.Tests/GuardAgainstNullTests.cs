using System;
using FluentAssertions;
using Xunit;
using Noundry.Guardian;

namespace Noundry.Guardian.Tests
{
    public class GuardAgainstNullTests
    {
        [Fact]
        public void Null_WhenValueIsNotNull_ReturnsValue()
        {
            // Arrange
            var value = "test";

            // Act
            var result = Guard.Against.Null(value);

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void Null_WhenValueIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            string? value = null;

            // Act & Assert
            var action = () => Guard.Against.Null(value);
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("value");
        }

        [Fact]
        public void Null_WithCustomMessage_ThrowsWithCustomMessage()
        {
            // Arrange
            string? value = null;
            var customMessage = "Custom error message";

            // Act & Assert
            var action = () => Guard.Against.Null(value, message: customMessage);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage($"*{customMessage}*");
        }

        [Fact]
        public void Null_WithNullableStruct_WhenHasValue_ReturnsValue()
        {
            // Arrange
            int? value = 42;

            // Act
            var result = Guard.Against.Null(value);

            // Assert
            result.Should().Be(42);
        }

        [Fact]
        public void Null_WithNullableStruct_WhenNull_ThrowsArgumentNullException()
        {
            // Arrange
            int? value = null;

            // Act & Assert
            var action = () => Guard.Against.Null(value);
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("value");
        }
    }
}