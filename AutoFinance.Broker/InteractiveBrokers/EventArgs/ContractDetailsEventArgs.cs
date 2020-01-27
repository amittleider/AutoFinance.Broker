// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    using IBApi;

    /// <summary>
    /// Contract Details Event Arguments
    /// </summary>
    public class ContractDetailsEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractDetailsEventArgs"/> class.
        /// </summary>
        /// <param name="requestId">The request ID from TWS</param>
        /// <param name="contractDetails">The Contract Details from TWS</param>
        public ContractDetailsEventArgs(int requestId, ContractDetails contractDetails)
        {
            this.RequestId = requestId;
            this.ContractDetails = contractDetails;
        }

        /// <summary>
        /// Gets this structure contains a full description of the contract being looked up.
        /// </summary>
        public ContractDetails ContractDetails
        {
            get;
            private set;
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
