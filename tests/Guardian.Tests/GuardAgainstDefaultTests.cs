using System;
using FluentAssertions;
using Xunit;
using Noundry.Guardian;

namespace Noundry.Guardian.Tests
{
    public class GuardAgainstDefaultTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(42)]
        [InlineData(-1)]
        public void Default_WithStruct_WhenNotDefault_ReturnsValue(int value)
        {
            // Act
            var result = Guard.Against.DefaultStruct(value);

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void Default_WithStruct_WhenDefault_ThrowsArgumentException()
        {
            // Act & Assert
            var action = () => Guard.Against.DefaultStruct(0);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Default_WithGuid_WhenNotEmpty_ReturnsValue()
        {
            // Arrange
            var value = Guid.NewGuid();

            // Act
            var result = Guard.Against.DefaultStruct(value);

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void Default_WithGuid_WhenEmpty_ThrowsArgumentException()
        {
            // Act & Assert
            var action = () => Guard.Against.DefaultStruct(Guid.Empty);
            action.Should().Throw<ArgumentException>()
                .WithMessage("*cannot be the default value*");
        }

        [Fact]
        public void Default_WithDateTime_WhenNotDefault_ReturnsValue()
        {
            // Arrange
            var value = DateTime.Now;

            // Act
            var result = Guard.Against.DefaultStruct(value);

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void Default_WithDateTime_WhenDefault_ThrowsArgumentException()
        {
            // Act & Assert
            var action = () => Guard.Against.DefaultStruct(default(DateTime));
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Default_WithClass_WhenNotNull_ReturnsValue()
        {
            // Arrange
            var value = "test";

            // Act
            var result = Guard.Against.Default(value);

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void Default_WithClass_WhenNull_ThrowsArgumentException()
        {
            // Arrange
            string? value = null;

            // Act & Assert
            var action = () => Guard.Against.Default(value);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Default_WithCustomStruct_WhenNotDefault_ReturnsValue()
        {
            // Arrange
            var value = new TestStruct { Value = 42 };

            // Act
            var result = Guard.Against.DefaultStruct(value);

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void Default_WithCustomStruct_WhenDefault_ThrowsArgumentException()
        {
            // Act & Assert
            var action = () => Guard.Against.DefaultStruct(default(TestStruct));
            action.Should().Throw<ArgumentException>();
        }

        private struct TestStruct : IEquatable<TestStruct>
        {
            public int Value { get; set; }

            public bool Equals(TestStruct other)
            {
                return Value == other.Value;
            }

            public override bool Equals(object? obj)
            {
                return obj is TestStruct other && Equals(other);
            }

            public override int GetHashCode()
            {
                return Value;
            }
        }
    }
}