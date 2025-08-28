using System;
using FluentAssertions;
using Xunit;

namespace Guardian.Tests
{
    public class GuardAgainstConditionTests
    {
        [Fact]
        public void Condition_WhenConditionIsTrue_DoesNotThrow()
        {
            // Act
            var action = () => Guard.Against.Condition(true);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Condition_WhenConditionIsFalse_ThrowsArgumentException()
        {
            // Act & Assert
            var action = () => Guard.Against.Condition(false);
            action.Should().Throw<ArgumentException>()
                .WithMessage("*Condition was not met*");
        }

        [Fact]
        public void Condition_WithParameterName_ThrowsWithParameterName()
        {
            // Act & Assert
            var action = () => Guard.Against.Condition(false, "myParameter");
            action.Should().Throw<ArgumentException>()
                .WithParameterName("myParameter");
        }

        [Fact]
        public void Condition_WithCustomMessage_ThrowsWithCustomMessage()
        {
            // Arrange
            var customMessage = "The business rule validation failed";

            // Act & Assert
            var action = () => Guard.Against.Condition(false, "myParameter", customMessage);
            action.Should().Throw<ArgumentException>()
                .WithMessage($"*{customMessage}*")
                .WithParameterName("myParameter");
        }

        [Fact]
        public void Condition_ComplexCondition_WhenTrue_DoesNotThrow()
        {
            // Arrange
            var value1 = 10;
            var value2 = 5;

            // Act
            var action = () => Guard.Against.Condition(value1 > value2, "values", "First value must be greater than second");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Condition_ComplexCondition_WhenFalse_ThrowsWithDetails()
        {
            // Arrange
            var value1 = 5;
            var value2 = 10;

            // Act & Assert
            var action = () => Guard.Against.Condition(value1 > value2, "values", "First value must be greater than second");
            action.Should().Throw<ArgumentException>()
                .WithMessage("*First value must be greater than second*")
                .WithParameterName("values");
        }
    }
}