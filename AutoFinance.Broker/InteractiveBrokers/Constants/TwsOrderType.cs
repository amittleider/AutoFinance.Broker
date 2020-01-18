// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

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
    }
}
