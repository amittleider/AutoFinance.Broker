// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The event arguments when market data type is received.
    /// </summary>
    public class MarketDataTypeEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketDataTypeEventArgs"/> class.
        /// </summary>
        /// <param name="reqId">The request identifier.</param>
        /// <param name="marketDataType">The market data type value.</param>
        public MarketDataTypeEventArgs(int reqId, int marketDataType)
        {
            this.RequestId = reqId;
            this.MarketDataType = marketDataType;
        }

        /// <summary>
        /// Gets the request id.
        /// </summary>
        public int RequestId { get; private set; }

        /// <summary>
        /// Gets the market data type value.
        /// </summary>
        public int MarketDataType { get; private set; }
    }
}
