// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System.Threading;

    /// <summary>
    /// Retreive a request ID for Interactive Brokers
    /// The request ID actually doesn't matter on the IB side, it is only to match up the responses on the client side.
    /// </summary>
    internal class TwsRequestIdGenerator : ITwsRequestIdGenerator
    {
        /// <summary>
        /// Holds the state of the request Id for TWS
        /// </summary>
        private static int requestId = 1;

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
