// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Constants
{
    /// <summary>
    /// Tws Order Types
    /// </summary>
    public class TwsOrderType
    {
        /// <summary>
        /// Limit order
        /// </summary>
        public const string Limit = "LMT";

        /// <summary>
        /// Peg to market order (Used to be PEG MKT but seems to have been changed, docs don't reflect it).
        /// </summary>
        public const string PegToMarket = "REL";

        /// <summary>
        /// Market roders
        /// </summary>
        public const string Market = "MKT";

        /// <summary>
        /// Stop loss
        /// </summary>
        public const string StopLoss = "STP";

        /// <summary>
        /// Stop limit
        /// </summary>
        public const string StopLimit = "STP LMT";
    }
}
