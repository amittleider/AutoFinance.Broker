// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Exceptions
{
    using System;

    /// <summary>
    /// A generic exception class for TWS exceptions
    /// </summary>
    public class TwsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwsException"/> class.
        /// </summary>
        /// <param name="message">The exception message</param>
        public TwsException(string message)
            : base(message)
        {
        }
    }
}
