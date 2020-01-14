// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The event args for a ContractDetailsEndEvent of the TwsCallbackHandler
    /// </summary>
    public class ContractDetailsEndEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractDetailsEndEventArgs"/> class.
        /// This class corresponds with the ContractDetailsEnd response of the TwsCallbackHandler
        /// </summary>
        /// <param name="requestId">The request ID that corresponds with this response.</param>
        public ContractDetailsEndEventArgs(int requestId)
        {
            this.RequestId = requestId;
        }

        /// <summary>
        /// Gets the request ID that corresponds with this response.
        /// </summary>
        public int RequestId
        {
            get;
            private set;
        }
    }
}
