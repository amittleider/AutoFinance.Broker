// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    using System.Collections.Generic;

    /// <summary>
    /// Event args for the security definition option TWS endpoint
    /// </summary>
    public class SecurityDefinitionOptionParameterEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityDefinitionOptionParameterEventArgs"/> class.
        /// </summary>
        /// <param name="reqId">The request ID</param>
        /// <param name="exchange">The exchange</param>
        /// <param name="underlyingConId">The contract ID</param>
        /// <param name="tradingClass">The trading class</param>
        /// <param name="multiplier">The multiplier</param>
        /// <param name="expirations">The expirations</param>
        /// <param name="strikes">The strikes</param>
        public SecurityDefinitionOptionParameterEventArgs(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes)
        {
            this.RequestId = reqId;
            this.Exchange = exchange;
            this.UnderlyingConId = underlyingConId;
            this.TradingClass = tradingClass;
            this.Multiplier = multiplier;
            this.Expirations = expirations;
            this.Strikes = strikes;
        }

        /// <summary>
        /// Gets the request ID
        /// </summary>
        public int RequestId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the exchange
        /// </summary>
        public string Exchange
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the underlying contract ID
        /// </summary>
        public int UnderlyingConId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the trading class
        /// </summary>
        public string TradingClass
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the multiplier
        /// </summary>
        public string Multiplier
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the expiration dates
        /// </summary>
        public HashSet<string> Expirations
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the strikes
        /// </summary>
        public HashSet<double> Strikes
        {
            get;
            private set;
        }
    }
}
