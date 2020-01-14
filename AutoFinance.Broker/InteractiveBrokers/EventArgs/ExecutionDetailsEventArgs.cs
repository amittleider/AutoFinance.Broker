// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    using IBApi;

    /// <summary>
    /// The execution details event arguments
    /// </summary>
    public class ExecutionDetailsEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionDetailsEventArgs"/> class.
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="contract">The contract</param>
        /// <param name="execution">The execution</param>
        public ExecutionDetailsEventArgs(int reqId, Contract contract, Execution execution)
        {
            this.RequestId = reqId;
            this.Contract = contract;
            this.Execution = execution;
        }

        /// <summary>
        /// Gets the request Id
        /// </summary>
        public int RequestId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the contract
        /// </summary>
        public Contract Contract
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the execution
        /// </summary>
        public Execution Execution
        {
            get;
            private set;
        }
    }
}
