// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Wrappers
{
    using IBApi;

    /// <summary>
    /// Event arguments for calls to the TWS Position APi.
    /// </summary>
    public class PositionStatusEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionStatusEventArgs"/> class.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="contract">The contract</param>
        /// <param name="position">The position quantity</param>
        /// <param name="averageCost">The average cost</param>
        public PositionStatusEventArgs(string account, Contract contract, decimal position, double averageCost)
        {
            this.Account = account;
            this.Contract = contract;
            this.Position = position;
            this.AverageCost = averageCost;
        }

        /// <summary>
        /// Gets the account
        /// </summary>
        public string Account
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Contract
        /// </summary>
        public Contract Contract
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the position
        /// </summary>
        public decimal Position
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the average cost
        /// </summary>
        public double AverageCost
        {
            get;
            private set;
        }
    }
}
