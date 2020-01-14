// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.Constants
{
    /// <summary>
    /// The ThingsToShow parameter of TWS provides the following options.
    /// </summary>
    public class TwsThingsToShow
    {
        /// <summary>
        /// The bid
        /// </summary>
        public const string Bid = "BID";

        /// <summary>
        /// The ask
        /// </summary>
        public const string Ask = "ASK";

        /// <summary>
        /// The midpoint
        /// </summary>
        public const string Midpoint = "MIDPOINT";

        /// <summary>
        /// The trade
        /// </summary>
        public const string Trades = "TRADES";
    }
}
