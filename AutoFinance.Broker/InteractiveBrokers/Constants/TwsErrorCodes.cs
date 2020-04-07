// Licensed under the Apache License, Version 2.0.

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

        /// <summary>
        /// Order can't be cancelled (possibly filled)
        /// </summary>
        public const int OrderCannotBeCancelled = 10148;

        /// <summary>
        /// Order can't be cancelled (possibly filled)
        /// </summary>
        public const int OrderCannotBeCancelled2 = 10147;
    }
}
