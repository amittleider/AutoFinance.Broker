// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The event arguments for empty events sent from TWS.
    /// </summary>
    public class RequestIdEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestIdEventArgs"/> class.
        /// </summary>
        /// <param name="requestId">The request Id</param>
        public RequestIdEventArgs(int requestId)
        {
            this.RequestId = requestId;
        }

        /// <summary>
        /// Gets the request Id
        /// </summary>
        public int RequestId
        {
            get;
            private set;
        }
    }
}
