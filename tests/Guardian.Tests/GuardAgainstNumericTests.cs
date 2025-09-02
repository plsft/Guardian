using System;
using FluentAssertions;
using Xunit;
using Noundry.Guardian;

namespace Noundry.Guardian.Tests
{
    public class GuardAgainstNumericTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(0)]
        public void Negative_WhenValueIsNotNegative_ReturnsValue(int value)
        {
            // Act
            var result = Guard.Against.Negative(value);

            // Assert
            result.Should().Be(value);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Negative_WhenValueIsNegative_ThrowsArgumentOutOfRangeException(int value)
        {
            // Act & Assert
            var action = () => Guard.Against.Negative(value);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(100)]
        public void Zero_WhenValueIsNotZero_ReturnsValue(int value)
        {
            // Act
            var result = Guard.Against.Zero(value);

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void Zero_WhenValueIsZero_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            var action = () => Guard.Against.Zero(0);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(0.1)]
        public void NegativeOrZero_WhenValueIsPositive_ReturnsValue(decimal value)
        {
            // Act
            var result = Guard.Against.NegativeOrZero(value);

            // Assert
            result.Should().Be(value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void NegativeOrZero_WhenValueIsNegativeOrZero_ThrowsArgumentOutOfRangeException(decimal value)
        {
            // Act & Assert
            var action = () => Guard.Against.NegativeOrZero(value);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Positive_WhenValueIsNotPositive_ReturnsValue(int value)
        {
            // Act
            var result = Guard.Against.Positive(value);

            // Assert
            result.Should().Be(value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        public void Positive_WhenValueIsPositive_ThrowsArgumentOutOfRangeException(int value)
        {
            // Act & Assert
            var action = () => Guard.Against.Positive(value);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(5, 1, 10)]
        [InlineData(1, 1, 10)]
        [InlineData(10, 1, 10)]
        public void OutOfRange_WhenValueIsInRange_ReturnsValue(int value, int min, int max)
        {
            // Act
            var result = Guard.Against.OutOfRange(value, min, max);

            // Assert
            result.Should().Be(value);
        }

        [Theory]
        [InlineData(0, 1, 10)]
        [InlineData(11, 1, 10)]
        [InlineData(-1, 1, 10)]
        public void OutOfRange_WhenValueIsOutOfRange_ThrowsArgumentOutOfRangeException(int value, int min, int max)
        {
            // Act & Assert
            var action = () => Guard.Against.OutOfRange(value, min, max);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(5, 10)]
        [InlineData(10, 10)]
        [InlineData(-1, 10)]
        public void GreaterThan_WhenValueIsNotGreaterThan_ReturnsValue(int value, int maximum)
        {
            // Act
            var result = Guard.Against.GreaterThan(value, maximum);

            // Assert
            result.Should().Be(value);
        }

        [Theory]
        [InlineData(11, 10)]
        [InlineData(100, 10)]
        public void GreaterThan_WhenValueIsGreaterThan_ThrowsArgumentOutOfRangeException(int value, int maximum)
        {
            // Act & Assert
            var action = () => Guard.Against.GreaterThan(value, maximum);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(5, 10)]
        [InlineData(9, 10)]
        public void GreaterThanOrEqualTo_WhenValueIsLessThan_ReturnsValue(int value, int maximum)
        {
            // Act
            var result = Guard.Against.GreaterThanOrEqualTo(value, maximum);

            // Assert
            result.Should().Be(value);
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(11, 10)]
        public void GreaterThanOrEqualTo_WhenValueIsGreaterThanOrEqual_ThrowsArgumentOutOfRangeException(int value, int maximum)
        {
            // Act & Assert
            var action = () => Guard.Against.GreaterThanOrEqualTo(value, maximum);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(10, 5)]
        [InlineData(5, 5)]
        [InlineData(100, 5)]
        public void LessThan_WhenValueIsNotLessThan_ReturnsValue(int value, int minimum)
        {
            // Act
            var result = Guard.Against.LessThan(value, minimum);

            // Assert
            result.Should().Be(value);
        }

        [Theory]
        [InlineData(4, 5)]
        [InlineData(-1, 5)]
        public void LessThan_WhenValueIsLessThan_ThrowsArgumentOutOfRangeException(int value, int minimum)
        {
            // Act & Assert
            var action = () => Guard.Against.LessThan(value, minimum);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(10, 5)]
        [InlineData(6, 5)]
        public void LessThanOrEqualTo_WhenValueIsGreaterThan_ReturnsValue(int value, int minimum)
        {
            // Act
            var result = Guard.Against.LessThanOrEqualTo(value, minimum);

            // Assert
            result.Should().Be(value);
        }

        [Theory]
        [InlineData(5, 5)]
        [InlineData(4, 5)]
        public void LessThanOrEqualTo_WhenValueIsLessThanOrEqual_ThrowsArgumentOutOfRangeException(int value, int minimum)
        {
            // Act & Assert
            var action = () => Guard.Against.LessThanOrEqualTo(value, minimum);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}