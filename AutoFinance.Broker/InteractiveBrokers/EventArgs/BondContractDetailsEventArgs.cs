// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    using IBApi;

    /// <summary>
    /// The event arguments when a bond contract is received from reqContractDetails
    /// </summary>
    public class BondContractDetailsEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BondContractDetailsEventArgs"/> class.
        /// </summary>
        /// <param name="reqId">The request identifier.</param>
        /// <param name="contractDetails">The bond contract details.</param>
        public BondContractDetailsEventArgs(int reqId, ContractDetails contractDetails)
        {
            this.RequestId = reqId;
            this.ContractDetails = contractDetails;
        }

        /// <summary>
        /// Gets the request id.
        /// </summary>
        public int RequestId { get; private set; }

        /// <summary>
        /// Gets the contract details of a bond.
        /// </summary>
        public ContractDetails ContractDetails { get; private set; }
    }
}
