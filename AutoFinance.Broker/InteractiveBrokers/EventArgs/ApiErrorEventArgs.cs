// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The event arguments when an api error is thrown
    /// </summary>
    public class ApiErrorEventArgs
    {
       /// <summary>
       /// Initializes a new instance of the <see cref="ApiErrorEventArgs"/> class.
       /// </summary>
       /// <param name="reqId">The request identifier.</param>
        public ApiErrorEventArgs(int reqId)
        {
            this.RequestId = reqId;
        }

        /// <summary>
        /// Gets the request id.
        /// </summary>
        public int RequestId { get; private set; }
    }
}
