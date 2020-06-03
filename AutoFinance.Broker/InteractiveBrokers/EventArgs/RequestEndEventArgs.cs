// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The event arguments when a request is completed
    /// </summary>
    public class RequestEndEventArgs
    {
       /// <summary>
       /// Initializes a new instance of the <see cref="RequestEndEventArgs"/> class.
       /// </summary>
       /// <param name="reqId">The request identifier.</param>
        public RequestEndEventArgs(int reqId)
        {
            this.RequestId = reqId;
        }

        /// <summary>
        /// Gets the request id.
        /// </summary>
        public int RequestId { get; private set; }
    }
}
