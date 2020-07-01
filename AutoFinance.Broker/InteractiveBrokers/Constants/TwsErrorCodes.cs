// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Constants
{
    /// <summary>
    /// The class that holds the error codes for TWS
    /// </summary>
    public class TwsErrorCodes
    {
        /// <summary>
        /// Ambiguous contract error code
        /// </summary>
        public const int AmbiguousContract = 200;

        /// <summary>
        /// An attempted order was rejected by the IB servers.
        /// </summary>
        public const int OrderRejected = 201;

        /// <summary>
        /// OrderCancelled error code
        /// </summary>
        public const int OrderCancelled = 202;

        /// <summary>
        /// Server error when reading an API client request.
        /// </summary>
        public const int ServerErrorWhenReadingApiClientRequest = 320;

        /// <summary>
        /// Server error when validating an API client request.
        /// </summary>
        public const int ServerErrorWhenValidatingApiClientRequest = 321;

        /// <summary>
        /// Server error when validating an API client request.
        /// </summary>
        public const int ServerErrorWhenProcessingApiClientRequest = 322;

        /// <summary>
        /// Invalid order type error code
        /// </summary>
        public const int InvalidOrderType = 387;

        /// <summary>
        /// Order message error (i.e Your order was repriced)
        /// </summary>
        public const int OrderMessageError = 399;

        /// <summary>
        /// Order can't be cancelled (possibly filled)
        /// </summary>
        public const int OrderCannotBeCancelled2 = 10147;

        /// <summary>
        /// Order can't be cancelled (possibly filled)
        /// </summary>
        public const int OrderCannotBeCancelled = 10148;
    }
}
