using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Guardian.Tests
{
    public class GuardAgainstCollectionTests
    {
        [Fact]
        public void NullOrEmpty_WhenCollectionHasItems_ReturnsCollection()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            var result = Guard.Against.NullOrEmpty(collection);

            // Assert
            result.Should().BeEquivalentTo(collection);
        }

        [Fact]
        public void NullOrEmpty_WhenCollectionIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            int[]? collection = null;

            // Act & Assert
            var action = () => Guard.Against.NullOrEmpty(collection);
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("collection");
        }

        [Fact]
        public void NullOrEmpty_WhenCollectionIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var collection = Array.Empty<int>();

            // Act & Assert
            var action = () => Guard.Against.NullOrEmpty(collection);
            action.Should().Throw<ArgumentException>()
                .WithParameterName("collection")
                .WithMessage("*cannot be empty*");
        }

        [Fact]
        public void NullOrEmpty_WithList_WhenHasItems_ReturnsCollection()
        {
            // Arrange
            var collection = new List<string> { "a", "b", "c" };

            // Act
            var result = Guard.Against.NullOrEmpty(collection);

            // Assert
            result.Should().BeEquivalentTo(collection);
        }

        [Fact]
        public void NullOrEmpty_WithEnumerable_WhenHasItems_ReturnsCollection()
        {
            // Arrange
            var collection = Enumerable.Range(1, 5);

            // Act
            var result = Guard.Against.NullOrEmpty(collection);

            // Assert
            result.Should().BeEquivalentTo(collection);
        }

        [Fact]
        public void NotOneOf_WhenValueIsInAllowedValues_ReturnsValue()
        {
            // Arrange
            var value = "apple";
            var allowedValues = new[] { "apple", "banana", "orange" };

            // Act
            var result = Guard.Against.NotOneOf(value, allowedValues);

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void NotOneOf_WhenValueIsNotInAllowedValues_ThrowsArgumentException()
        {
            // Arrange
            var value = "grape";
            var allowedValues = new[] { "apple", "banana", "orange" };

            // Act & Assert
            var action = () => Guard.Against.NotOneOf(value, allowedValues);
            action.Should().Throw<ArgumentException>()
                .WithParameterName("value")
                .WithMessage("*must be one of*");
        }

        [Fact]
        public void NotOneOf_WithNumbers_WhenValueIsInAllowedValues_ReturnsValue()
        {
            // Arrange
            var value = 5;
            var allowedValues = new[] { 1, 3, 5, 7, 9 };

            // Act
            var result = Guard.Against.NotOneOf(value, allowedValues);

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void NotOneOf_WithNumbers_WhenValueIsNotInAllowedValues_ThrowsArgumentException()
        {
            // Arrange
            var value = 6;
            var allowedValues = new[] { 1, 3, 5, 7, 9 };

            // Act & Assert
            var action = () => Guard.Against.NotOneOf(value, allowedValues);
            action.Should().Throw<ArgumentException>()
                .WithParameterName("value");
        }
    }
}