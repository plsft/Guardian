using System;
using System.Runtime.CompilerServices;

namespace Guardian
{
    /// <summary>
    /// Extension methods for Guard clauses to provide a unified API.
    /// </summary>
    public static class GuardExtensions
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the value is default for its type.
        /// This is a convenience method that calls either DefaultStruct or Default based on the type.
        /// </summary>
        /// <typeparam name="T">The type of the value to check.</typeparam>
        /// <param name="guardClause">The guard clause instance.</param>
        /// <param name="value">The value to check.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        /// <param name="message">Optional custom error message.</param>
        /// <returns>The original value if not default.</returns>
        public static T Default<T>(this Guard.IGuardClause guardClause, T value, 
            [CallerArgumentExpression("value")] string? parameterName = null, string? message = null) where T : struct
        {
            return guardClause.DefaultStruct(value, parameterName, message);
        }
    }
}