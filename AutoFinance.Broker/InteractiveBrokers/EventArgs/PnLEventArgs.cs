// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Wrappers
{
    using IBApi;

    /// <summary>
    /// The event arguments for PnL events sent from TWS.
    /// </summary>
    public class PnLEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PnLEventArgs"/> class.
        /// </summary>
        /// <param name="requestId">The requestId</param>
        /// <param name="dailyPnL">The daily PnL</param>
        /// <param name="unrealizedPnL">The unrealized PnL</param>
        /// <param name="realizedPnL">The realized PnL</param>
        public PnLEventArgs(int requestId, double dailyPnL, double unrealizedPnL, double realizedPnL)
        {
            this.RequestId = requestId;
            this.DailyPnL = dailyPnL;
            this.UnrealizedPnL = unrealizedPnL;
            this.RealizedPnL = realizedPnL;
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
    }
}
