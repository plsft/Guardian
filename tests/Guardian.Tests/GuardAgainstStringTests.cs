using System;
using FluentAssertions;
using Xunit;

namespace Guardian.Tests
{
    public class GuardAgainstStringTests
    {
        [Theory]
        [InlineData("test")]
        [InlineData("  test  ")]
        [InlineData("a")]
        public void NullOrWhiteSpace_WhenValueIsValid_ReturnsValue(string value)
        {
            // Act
            var result = Guard.Against.NullOrWhiteSpace(value);

            // Assert
            result.Should().Be(value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        public void NullOrWhiteSpace_WhenInvalid_ThrowsArgumentException(string? value)
        {
            // Act & Assert
            var action = () => Guard.Against.NullOrWhiteSpace(value);
            action.Should().Throw<ArgumentException>()
                .WithParameterName("value");
        }

        [Theory]
        [InlineData("test")]
        [InlineData("  test  ")]
        [InlineData(" ")]
        public void NullOrEmpty_WhenValueIsValid_ReturnsValue(string value)
        {
            // Act
            var result = Guard.Against.NullOrEmpty(value);

            // Assert
            result.Should().Be(value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void NullOrEmpty_WhenInvalid_ThrowsArgumentException(string? value)
        {
            // Act & Assert
            var action = () => Guard.Against.NullOrEmpty(value);
            action.Should().Throw<ArgumentException>()
                .WithParameterName("value");
        }

        [Fact]
        public void InvalidFormat_WhenMatchesPattern_ReturnsValue()
        {
            // Arrange
            var email = "test@example.com";
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            // Act
            var result = Guard.Against.InvalidFormat(email, pattern);

            // Assert
            result.Should().Be(email);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("@example.com")]
        [InlineData("test@")]
        [InlineData("test@.com")]
        public void InvalidFormat_WhenDoesNotMatchPattern_ThrowsArgumentException(string value)
        {
            // Arrange
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            // Act & Assert
            var action = () => Guard.Against.InvalidFormat(value, pattern);
            action.Should().Throw<ArgumentException>()
                .WithParameterName("value");
        }

        [Theory]
        [InlineData("ab", 2, 10)]
        [InlineData("test", 2, 10)]
        [InlineData("1234567890", 2, 10)]
        public void InvalidLength_WhenLengthIsValid_ReturnsValue(string value, int min, int max)
        {
            // Act
            var result = Guard.Against.InvalidLength(value, min, max);

            // Assert
            result.Should().Be(value);
        }

        [Theory]
        [InlineData("a", 2, 10)]
        [InlineData("", 2, 10)]
        [InlineData("12345678901", 2, 10)]
        public void InvalidLength_WhenLengthIsInvalid_ThrowsArgumentOutOfRangeException(string value, int min, int max)
        {
            // Act & Assert
            var action = () => Guard.Against.InvalidLength(value, min, max);
            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("value");
        }
    }
}