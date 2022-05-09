using System;
using System.Runtime.CompilerServices;

namespace CMP1903M_Assessment_2
{
    /// <summary>
    /// A custom exception class for invalid die values.
    /// </summary>
    public class DieValueNotValidException : Exception
    {
        /// <summary>
        /// Default exception message.
        /// </summary>
        public DieValueNotValidException() : base("Invalid die face value used.") { }

        /// <summary>
        /// Exception message when an unacceptable value is included.
        /// </summary>
        /// <param name="value">The level passed to the function.</param>
        /// <param name="callerName">The method calling the exception - provided by default.</param>
        public DieValueNotValidException(int value, [CallerMemberName] string callerName = "") 
            : base($"Die value {value} is not valid for a {callerName} die.") { }
    }
}