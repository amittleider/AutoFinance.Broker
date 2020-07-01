// Licensed under the Apache License, Version 2.0.

using AutoFinance.Broker.InteractiveBrokers.EventArgs;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsException"/> class.
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="errorCode">The TWS error code associated to this exception</param>
        /// <param name="id">The id associated to this error.</param>
        public TwsException(string message, int errorCode, int? id)
            : base(message)
        {
            this.ErrorCode = errorCode;
            this.Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsException"/> class.
        /// </summary>
        /// <param name="errorEventArgs">An ErrorEventArgs instance raised by TWS</param>
        public TwsException(ErrorEventArgs errorEventArgs)
            : base(errorEventArgs.ErrorMessage)
        {
            this.ErrorCode = errorEventArgs.ErrorCode;
            this.Id = errorEventArgs.Id;
        }

        /// <summary>
        /// Gets the error code associated to this exception
        /// </summary>
        public int? ErrorCode { get; }

        /// <summary>
        /// Gets the request id associated to this error.
        /// </summary>
        public int? Id { get; }
    }
}
