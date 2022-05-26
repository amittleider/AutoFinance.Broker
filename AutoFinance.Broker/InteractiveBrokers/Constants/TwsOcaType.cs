// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Constants
{
    /// <summary>
    /// Oca type used for One Cancels All orders
    /// </summary>
    public enum TwsOcaType
    {
        /// <summary>
        /// Cancel all remaining orders with block.*
        /// Note*: if you use a value "with block" it gives the order overfill protection.
        /// This means that only one order in the group will be routed at a time to remove the possibility of an overfill.
        /// </summary>
        CancelAllRemainingOrdersWithBlock = 0,

        /// <summary>
        /// Remaining orders are proportionately reduced in size with block.*
        /// Note*: if you use a value "with block" it gives the order overfill protection.
        /// This means that only one order in the group will be routed at a time to remove the possibility of an overfill.
        /// </summary>
        ProportionallyReduceSizeWithBlock = 1,

        /// <summary>
        /// Remaining orders are proportionately reduced in size with no block.
        /// Note*: if you use a value "with block" it gives the order overfill protection.
        /// This means that only one order in the group will be routed at a time to remove the possibility of an overfill.
        /// </summary>
        ProportionallyReduceSizeWithoutBlock = 2,
    }
}
