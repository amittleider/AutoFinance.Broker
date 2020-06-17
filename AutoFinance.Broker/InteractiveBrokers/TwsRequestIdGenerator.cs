// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System.Threading;

    /// <summary>
    /// Retreive a request ID for Interactive Brokers
    /// The request ID actually doesn't matter on the IB side, it is only to match up the responses on the client side.
    /// </summary>
    public class TwsRequestIdGenerator : ITwsRequestIdGenerator
    {
        /// <summary>
        /// Holds the state of the request Id for TWS
        /// </summary>
        private static int requestId = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsRequestIdGenerator"/> class.
        /// Generates a request Id for TWS requests
        /// </summary>
        public TwsRequestIdGenerator()
        {
        }

        /// <summary>
        /// Get the next request ID for IB
        /// The method is thread safe.
        /// </summary>
        /// <returns>Gets the next request ID for IB.</returns>
        public int GetNextRequestId()
        {
            return Interlocked.Increment(ref requestId);
        }
    }
}
