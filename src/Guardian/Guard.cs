using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

#if !NETSTANDARD2_0 && !NETSTANDARD2_1
using System.Runtime.CompilerServices;
#endif

namespace Guardian
{
    /// <summary>
    /// Entry point for guard clauses that provide parameter validation.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Provides access to all guard clauses for parameter validation.
        /// </summary>
        public static IGuardClause Against { get; } = new GuardClause();

        /// <summary>
        /// Interface defining all available guard clause methods.
        /// </summary>
        public interface IGuardClause
        {
            /// <summary>
            /// Throws an <see cref="ArgumentNullException"/> if the value is null.
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if not null.</returns>
            [return: NotNull]
            T Null<T>([NotNull] T? value, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : class;

            /// <summary>
            /// Throws an <see cref="ArgumentNullException"/> if the value is null.
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if not null.</returns>
            [return: NotNull]
            T Null<T>([NotNull] T? value, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : struct;

            /// <summary>
            /// Throws an <see cref="ArgumentException"/> if the string is null or whitespace.
            /// </summary>
            /// <param name="value">The string to check.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if not null or whitespace.</returns>
            [return: NotNull]
            string NullOrWhiteSpace([NotNull] string? value, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null);

            /// <summary>
            /// Throws an <see cref="ArgumentException"/> if the string is null or empty.
            /// </summary>
            /// <param name="value">The string to check.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if not null or empty.</returns>
            [return: NotNull]
            string NullOrEmpty([NotNull] string? value, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null);

            /// <summary>
            /// Throws an <see cref="ArgumentException"/> if the value is default for its type (struct).
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if not default.</returns>
            T DefaultStruct<T>(T value, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : struct;

            /// <summary>
            /// Throws an <see cref="ArgumentException"/> if the value is default for its type (class).
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if not default.</returns>
            [return: NotNull]
            T Default<T>([NotNull] T? value, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : class;

            /// <summary>
            /// Throws an <see cref="ArgumentOutOfRangeException"/> if the value is negative.
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if not negative.</returns>
            T Negative<T>(T value, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : IComparable<T>;

            /// <summary>
            /// Throws an <see cref="ArgumentOutOfRangeException"/> if the value is zero.
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if not zero.</returns>
            T Zero<T>(T value, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : IComparable<T>;

            /// <summary>
            /// Throws an <see cref="ArgumentOutOfRangeException"/> if the value is negative or zero.
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if positive.</returns>
            T NegativeOrZero<T>(T value, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : IComparable<T>;

            /// <summary>
            /// Throws an <see cref="ArgumentOutOfRangeException"/> if the value is positive.
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if not positive.</returns>
            T Positive<T>(T value, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : IComparable<T>;

            /// <summary>
            /// Throws an <see cref="ArgumentOutOfRangeException"/> if the value is outside the specified range.
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="min">The minimum allowed value.</param>
            /// <param name="max">The maximum allowed value.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if within range.</returns>
            T OutOfRange<T>(T value, T min, T max, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : IComparable<T>;

            /// <summary>
            /// Throws an <see cref="ArgumentException"/> if the value is not a defined value of the specified enum type.
            /// </summary>
            /// <typeparam name="TEnum">The enum type.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if it is a defined enum value.</returns>
            TEnum NotInEnum<TEnum>(TEnum value, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where TEnum : struct, Enum;

            /// <summary>
            /// Throws an <see cref="ArgumentException"/> if the collection is null or empty.
            /// </summary>
            /// <typeparam name="T">The type of items in the collection.</typeparam>
            /// <param name="collection">The collection to check.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original collection if not null or empty.</returns>
            [return: NotNull]
            IEnumerable<T> NullOrEmpty<T>([NotNull] IEnumerable<T>? collection, [CallerArgumentExpression("collection")] string? parameterName = null, string? message = null);

            /// <summary>
            /// Throws an <see cref="ArgumentException"/> if the condition is false.
            /// </summary>
            /// <param name="condition">The condition to check.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">The error message to throw if the condition is false.</param>
            void Condition(bool condition, string? parameterName = null, string? message = null);

            /// <summary>
            /// Throws an <see cref="ArgumentException"/> if the string doesn't match the specified regex pattern.
            /// </summary>
            /// <param name="value">The string to check.</param>
            /// <param name="pattern">The regex pattern to match.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if it matches the pattern.</returns>
            [return: NotNull]
            string InvalidFormat([NotNull] string? value, string pattern, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null);

            /// <summary>
            /// Throws an <see cref="ArgumentOutOfRangeException"/> if the string length is outside the specified range.
            /// </summary>
            /// <param name="value">The string to check.</param>
            /// <param name="minLength">The minimum allowed length.</param>
            /// <param name="maxLength">The maximum allowed length.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if its length is within range.</returns>
            [return: NotNull]
            string InvalidLength([NotNull] string? value, int minLength, int maxLength, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null);

            /// <summary>
            /// Throws an <see cref="ArgumentException"/> if the value is not one of the allowed values.
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="allowedValues">The allowed values.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if it is one of the allowed values.</returns>
            T NotOneOf<T>(T value, IEnumerable<T> allowedValues, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null);

            /// <summary>
            /// Throws an <see cref="ArgumentOutOfRangeException"/> if the value is greater than the maximum.
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="maximum">The maximum allowed value.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if not greater than maximum.</returns>
            T GreaterThan<T>(T value, T maximum, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : IComparable<T>;

            /// <summary>
            /// Throws an <see cref="ArgumentOutOfRangeException"/> if the value is greater than or equal to the maximum.
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="maximum">The maximum allowed value.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if less than maximum.</returns>
            T GreaterThanOrEqualTo<T>(T value, T maximum, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : IComparable<T>;

            /// <summary>
            /// Throws an <see cref="ArgumentOutOfRangeException"/> if the value is less than the minimum.
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="minimum">The minimum allowed value.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if not less than minimum.</returns>
            T LessThan<T>(T value, T minimum, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : IComparable<T>;

            /// <summary>
            /// Throws an <see cref="ArgumentOutOfRangeException"/> if the value is less than or equal to the minimum.
            /// </summary>
            /// <typeparam name="T">The type of the value to check.</typeparam>
            /// <param name="value">The value to check.</param>
            /// <param name="minimum">The minimum allowed value.</param>
            /// <param name="parameterName">The name of the parameter being checked.</param>
            /// <param name="message">Optional custom error message.</param>
            /// <returns>The original value if greater than minimum.</returns>
            T LessThanOrEqualTo<T>(T value, T minimum, [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : IComparable<T>;
        }

        private sealed class GuardClause : IGuardClause
        {
            public T Null<T>(T? value, string? parameterName, string? message) where T : class
            {
                if (value is null)
                {
                    throw new ArgumentNullException(parameterName, message);
                }
                return value;
            }

            public T Null<T>(T? value, string? parameterName, string? message) where T : struct
            {
                if (!value.HasValue)
                {
                    throw new ArgumentNullException(parameterName, message);
                }
                return value.Value;
            }

            public string NullOrWhiteSpace(string? value, string? parameterName, string? message)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(message ?? "Value cannot be null or whitespace.", parameterName);
                }
                return value;
            }

            public string NullOrEmpty(string? value, string? parameterName, string? message)
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(message ?? "Value cannot be null or empty.", parameterName);
                }
                return value;
            }

            public T DefaultStruct<T>(T value, string? parameterName, string? message) where T : struct
            {
                if (EqualityComparer<T>.Default.Equals(value, default))
                {
                    throw new ArgumentException(message ?? $"Value cannot be the default value for {typeof(T).Name}.", parameterName);
                }
                return value;
            }

            public T Default<T>(T? value, string? parameterName, string? message) where T : class
            {
                if (value is null || EqualityComparer<T>.Default.Equals(value, default))
                {
                    throw new ArgumentException(message ?? $"Value cannot be the default value for {typeof(T).Name}.", parameterName);
                }
                return value;
            }

            public T Negative<T>(T value, string? parameterName, string? message) where T : IComparable<T>
            {
                var zero = GetZero<T>();
                if (value.CompareTo(zero) < 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName, value, message ?? "Value cannot be negative.");
                }
                return value;
            }

            public T Zero<T>(T value, string? parameterName, string? message) where T : IComparable<T>
            {
                var zero = GetZero<T>();
                if (value.CompareTo(zero) == 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName, value, message ?? "Value cannot be zero.");
                }
                return value;
            }

            public T NegativeOrZero<T>(T value, string? parameterName, string? message) where T : IComparable<T>
            {
                var zero = GetZero<T>();
                if (value.CompareTo(zero) <= 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName, value, message ?? "Value must be positive (greater than zero).");
                }
                return value;
            }

            public T Positive<T>(T value, string? parameterName, string? message) where T : IComparable<T>
            {
                var zero = GetZero<T>();
                if (value.CompareTo(zero) > 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName, value, message ?? "Value cannot be positive.");
                }
                return value;
            }

            public T OutOfRange<T>(T value, T min, T max, string? parameterName, string? message) where T : IComparable<T>
            {
                if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName, value, message ?? $"Value must be between {min} and {max}.");
                }
                return value;
            }

            public TEnum NotInEnum<TEnum>(TEnum value, string? parameterName, string? message) where TEnum : struct, Enum
            {
                if (!Enum.IsDefined(typeof(TEnum), value))
                {
                    throw new ArgumentException(message ?? $"Value {value} is not defined in enum {typeof(TEnum).Name}.", parameterName);
                }
                return value;
            }

            public IEnumerable<T> NullOrEmpty<T>(IEnumerable<T>? collection, string? parameterName, string? message)
            {
                if (collection is null)
                {
                    throw new ArgumentNullException(parameterName, message);
                }

                var enumerable = collection as T[] ?? collection.ToArray();
                if (!enumerable.Any())
                {
                    throw new ArgumentException(message ?? "Collection cannot be empty.", parameterName);
                }

                return enumerable;
            }

            public void Condition(bool condition, string? parameterName, string? message)
            {
                if (!condition)
                {
                    throw new ArgumentException(message ?? "Condition was not met.", parameterName);
                }
            }

            public string InvalidFormat(string? value, string pattern, string? parameterName, string? message)
            {
                value = Null(value, parameterName, message);

                if (!Regex.IsMatch(value, pattern))
                {
                    throw new ArgumentException(message ?? $"Value does not match the required format: {pattern}", parameterName);
                }

                return value;
            }

            public string InvalidLength(string? value, int minLength, int maxLength, string? parameterName, string? message)
            {
                value = Null(value, parameterName, message);

                if (value.Length < minLength || value.Length > maxLength)
                {
                    throw new ArgumentOutOfRangeException(parameterName, value.Length, message ?? $"String length must be between {minLength} and {maxLength}.");
                }

                return value;
            }

            public T NotOneOf<T>(T value, IEnumerable<T> allowedValues, string? parameterName, string? message)
            {
                var allowed = allowedValues as T[] ?? allowedValues.ToArray();
                if (!allowed.Contains(value))
                {
                    throw new ArgumentException(message ?? $"Value must be one of: {string.Join(", ", allowed)}", parameterName);
                }

                return value;
            }

            public T GreaterThan<T>(T value, T maximum, string? parameterName, string? message) where T : IComparable<T>
            {
                if (value.CompareTo(maximum) > 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName, value, message ?? $"Value must not be greater than {maximum}.");
                }
                return value;
            }

            public T GreaterThanOrEqualTo<T>(T value, T maximum, string? parameterName, string? message) where T : IComparable<T>
            {
                if (value.CompareTo(maximum) >= 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName, value, message ?? $"Value must be less than {maximum}.");
                }
                return value;
            }

            public T LessThan<T>(T value, T minimum, string? parameterName, string? message) where T : IComparable<T>
            {
                if (value.CompareTo(minimum) < 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName, value, message ?? $"Value must not be less than {minimum}.");
                }
                return value;
            }

            public T LessThanOrEqualTo<T>(T value, T minimum, string? parameterName, string? message) where T : IComparable<T>
            {
                if (value.CompareTo(minimum) <= 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName, value, message ?? $"Value must be greater than {minimum}.");
                }
                return value;
            }

            private static T GetZero<T>() where T : IComparable<T>
            {
                if (typeof(T) == typeof(int)) return (T)(object)0;
                if (typeof(T) == typeof(long)) return (T)(object)0L;
                if (typeof(T) == typeof(short)) return (T)(object)(short)0;
                if (typeof(T) == typeof(byte)) return (T)(object)(byte)0;
                if (typeof(T) == typeof(uint)) return (T)(object)0U;
                if (typeof(T) == typeof(ulong)) return (T)(object)0UL;
                if (typeof(T) == typeof(ushort)) return (T)(object)(ushort)0;
                if (typeof(T) == typeof(sbyte)) return (T)(object)(sbyte)0;
                if (typeof(T) == typeof(float)) return (T)(object)0f;
                if (typeof(T) == typeof(double)) return (T)(object)0d;
                if (typeof(T) == typeof(decimal)) return (T)(object)0m;
                
                return default!;
            }
        }
    }
}