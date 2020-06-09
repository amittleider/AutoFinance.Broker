// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// Events container for data from the tickEFP TWS endpoint
    /// </summary>
    public class TickEFPEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickEFPEventArgs"/> class.
        /// </summary>
        /// <param name="tickerId">The ticker id</param>
        /// <param name="tickType">The type</param>
        /// <param name="basisPoints">The annualized rate value in basis points</param>
        /// <param name="formattedBasisPoints">The annualized rate value in basis points in percentage form</param>
        /// <param name="impliedFuture">The implied futures price</param>
        /// <param name="holdDays">The number of hold days until the lastTradeDate of the EFP</param>
        /// <param name="futureLastTradeDate">The expiration date of the single stock future</param>
        /// <param name="dividendImpact">The dividend impact upon the annualized basis points interest rate</param>
        /// <param name="dividendsToLastTrade">The dividends expected until the expiration of the single stock future</param>
        public TickEFPEventArgs(
            int tickerId,
            int tickType,
            double basisPoints,
            string formattedBasisPoints,
            double impliedFuture,
            int holdDays,
            string futureLastTradeDate,
            double dividendImpact,
            double dividendsToLastTrade)
        {
            this.TickerId = tickerId;
            this.TickType = tickType;
            this.BasisPoints = basisPoints;
            this.FormattedBasisPoints = formattedBasisPoints;
            this.ImpliedFuture = impliedFuture;
            this.HoldDays = holdDays;
            this.FutureLastTradeDate = futureLastTradeDate;
            this.DividendImpact = dividendImpact;
            this.DividendsToLastTradeDate = dividendsToLastTrade;
        }

        /// <summary>
        /// Gets the ticker id
        /// </summary>
        public int TickerId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type
        /// </summary>
        public int TickType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the annualized rate value in basis points
        /// </summary>
        public double BasisPoints
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the annualized rate value in basis points in percentage form
        /// </summary>
        public string FormattedBasisPoints
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the implied futures price
        /// </summary>
        public double ImpliedFuture
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of hold days until the lastTradeDate of the EFP
        /// </summary>
        public int HoldDays
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the expiration date of the single stock future
        /// </summary>
        public string FutureLastTradeDate
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the dividend impact upon the annualized basis points interest rate
        /// </summary>
        public double DividendImpact
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the dividends expected until the expiration of the single stock future
        /// </summary>
        public double DividendsToLastTradeDate
        {
            get;
            private set;
        }
    }
}
