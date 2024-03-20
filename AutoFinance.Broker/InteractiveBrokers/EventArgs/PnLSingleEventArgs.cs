// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Wrappers
{
    using IBApi;

    /// <summary>
    /// The event arguments for PnL Single position events sent from TWS.
    /// </summary>
    public class PnLSingleEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PnLSingleEventArgs"/> class.
        /// </summary>
        /// <param name="requestId">The request Id</param>
        /// <param name="position">The position size</param>
        /// <param name="dailyPnL">The daily PnL</param>
        /// <param name="unrealizedPnL">The unrealized PnL</param>
        /// <param name="realizedPnL">The realized PnL</param>
        /// <param name="value">The position value</param>
        public PnLSingleEventArgs(int requestId, decimal position, double dailyPnL, double unrealizedPnL, double realizedPnL, double value)
        {
            this.RequestId = requestId;
            this.Position = position;
            this.DailyPnL = dailyPnL;
            this.UnrealizedPnL = unrealizedPnL;
            this.RealizedPnL = realizedPnL;
            this.Value = value;
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
        /// Gets the position size
        /// </summary>
        public decimal Position
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the daily PnL
        /// </summary>
        public double DailyPnL
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the unrealized PnL
        /// </summary>
        public double UnrealizedPnL
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the realized PnL
        /// </summary>
        public double RealizedPnL
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the position value
        /// </summary>
        public double Value
        {
            get;
            private set;
        }
    }
}
