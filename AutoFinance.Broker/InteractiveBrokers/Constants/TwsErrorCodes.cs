// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.Constants
{
    /// <summary>
    /// The class that holds the error codes for TWS
    /// </summary>
    public class TwsErrorCodes
    {
        /// <summary>
        /// OrderCancelled error code
        /// </summary>
        public const int OrderCancelled = 202;

        /// <summary>
        /// Invalid order type error code
        /// </summary>
        public const int InvalidOrderType = 387;

        /// <summary>
        /// Ambiguous contract error code
        /// </summary>
        public const int AmbiguousContract = 200;
    }
}
