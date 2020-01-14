// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The execution details end event arguments
    /// </summary>
    public class ExecutionDetailsEndEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionDetailsEndEventArgs"/> class.
        /// </summary>
        /// <param name="requestId">The request Id</param>
        public ExecutionDetailsEndEventArgs(int requestId)
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
