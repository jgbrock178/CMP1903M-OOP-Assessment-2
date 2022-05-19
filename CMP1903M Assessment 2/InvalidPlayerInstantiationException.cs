using System;
using System.Runtime.CompilerServices;

namespace CMP1903M_Assessment_2
{
    /// <summary>
    /// A custom exception for when player is instantiated without the required properties.
    /// </summary>
    public class InvalidPlayerInstantiationException : Exception
    {
        /// <summary>
        /// Default exception message.
        /// </summary>
        public InvalidPlayerInstantiationException([CallerMemberName] string callerName = "") : base(
            $"{callerName} must be instantiated with a UI and Player Number.") { }
    }
}