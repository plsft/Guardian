using System;
using FluentAssertions;
using Xunit;
using Noundry.Guardian;

namespace Noundry.Guardian.Tests
{
    public class GuardAgainstEnumTests
    {
        public enum TestEnum
        {
            Value1 = 1,
            Value2 = 2,
            Value3 = 3
        }

        [Theory]
        [InlineData(TestEnum.Value1)]
        [InlineData(TestEnum.Value2)]
        [InlineData(TestEnum.Value3)]
        public void NotInEnum_WhenValueIsDefined_ReturnsValue(TestEnum value)
        {
            // Act
            var result = Guard.Against.NotInEnum(value);

            // Assert
            result.Should().Be(value);
        }

        [Theory]
        [InlineData((TestEnum)0)]
        [InlineData((TestEnum)4)]
        [InlineData((TestEnum)999)]
        public void NotInEnum_WhenValueIsNotDefined_ThrowsArgumentException(TestEnum value)
        {
            // Act & Assert
            var action = () => Guard.Against.NotInEnum(value);
            action.Should().Throw<ArgumentException>()
                .WithParameterName("value")
                .WithMessage($"*{value}*is not defined in enum*");
        }

        [Fact]
        public void NotInEnum_WithCustomMessage_ThrowsWithCustomMessage()
        {
            // Arrange
            var value = (TestEnum)999;
            var customMessage = "Invalid enum value provided";

            // Act & Assert
            var action = () => Guard.Against.NotInEnum(value, message: customMessage);
            action.Should().Throw<ArgumentException>()
                .WithMessage($"*{customMessage}*");
        }
    }
}