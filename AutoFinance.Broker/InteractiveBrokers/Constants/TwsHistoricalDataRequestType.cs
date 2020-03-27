// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Constants
{
    /// <summary>
    /// The TWS historical data request type, which is called 'whatToShow' in IB parameter naming convention
    /// </summary>
    public class TwsHistoricalDataRequestType : ITwsStringParameter
    {
        /// <summary>
        /// Bid
        /// </summary>
        private static TwsHistoricalDataRequestType bid = new TwsHistoricalDataRequestType("BID");

        /// <summary>
        /// Ask
        /// </summary>
        private static TwsHistoricalDataRequestType ask = new TwsHistoricalDataRequestType("ASK");

        /// <summary>
        /// Midpoint
        /// </summary>
        private static TwsHistoricalDataRequestType midpoint = new TwsHistoricalDataRequestType("MIDPOINT");

        /// <summary>
        /// Trade
        /// </summary>
        private static TwsHistoricalDataRequestType trade = new TwsHistoricalDataRequestType("TRADE");

        /// <summary>
        /// The underlying string
        /// </summary>
        private readonly string type;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsHistoricalDataRequestType"/> class.
        /// </summary>
        /// <param name="type">The underlying string</param>
        private TwsHistoricalDataRequestType(string type)
        {
            this.type = type;
        }

        /// <summary>
        /// Gets bid
        /// </summary>
        public static TwsHistoricalDataRequestType Bid { get => bid; }

        /// <summary>
        /// Gets ask
        /// </summary>
        public static TwsHistoricalDataRequestType Ask { get => ask; }

        /// <summary>
        /// Gets midpoint
        /// </summary>
        public static TwsHistoricalDataRequestType Midpoint { get => midpoint; }

        /// <summary>
        /// Gets trade
        /// </summary>
        public static TwsHistoricalDataRequestType Trade { get => trade; }

        /// <summary>
        /// The string version of the parameter when sending to the TWS API
        /// </summary>
        /// <returns>A string for the TWS API</returns>
        public string ToTwsParameter()
        {
            return this.type;
        }
    }
}
