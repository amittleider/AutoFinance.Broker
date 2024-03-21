// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    using IBApi;

    /// <summary>
    /// The event arguments when a request is completed
    /// </summary>
    public class PositionMultiEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionMultiEventArgs"/> class.
        /// </summary>
        /// <param name="reqId">The request identifier.</param>
        /// <param name="account">The account name.</param>
        /// <param name="modelCode">The model code.</param>
        /// <param name="contract">The contract information.</param>
        /// <param name="position">The position value.</param>
        /// <param name="averageCost">The average cost.</param>
        public PositionMultiEventArgs(int reqId, string account, string modelCode, Contract contract, decimal position, double averageCost)
       {
           this.RequestId = reqId;
           this.Account = account;
           this.ModelCode = modelCode;
           this.Contract = contract;
           this.Position = position;
           this.AverageCost = averageCost;
       }

        /// <summary>
        /// Gets the request id.
        /// </summary>
        public int RequestId { get; private set; }

        /// <summary>
        /// Gets the account name
        /// </summary>
        public string Account { get; private set; }

        /// <summary>
        /// Gets the model code.
        /// </summary>
        public string ModelCode { get; private set; }

        /// <summary>
        /// Gets the contract information
        /// </summary>
        public Contract Contract { get; private set; }

        /// <summary>
        /// Gets the position value.
        /// </summary>
        public decimal Position { get; private set; }

        /// <summary>
        /// Gets the average cost.
        /// </summary>
        public double AverageCost { get; private set; }
    }
}
