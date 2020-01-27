// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Constants
{
    /// <summary>
    /// The TWS historical data request type, which is called 'whatToShow' in IB parameter naming convention
    /// </summary>
    public class TwsHistoricalDataRequestType
    {
        /// <summary>
        /// Bid type
        /// </summary>
        public const string Bid = "BID";

        /// <summary>
        /// Ask type
        /// </summary>
        public const string Ask = "ASK";

        /// <summary>
        /// Midpoint type
        /// </summary>
        public const string Midpoint = "MIDPOINT";

        /// <summary>
        /// Trade type
        /// </summary>
        public const string Trade = "TRADE";
    }
}
