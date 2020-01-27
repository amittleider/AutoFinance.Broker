// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The arguments for the NextValidId event in TWS, which is referring to the next valid Order Id
    /// </summary>
    public class NextValidIdEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NextValidIdEventArgs"/> class.
        /// </summary>
        /// <param name="orderId">The order Id</param>
        public NextValidIdEventArgs(int orderId)
        {
            this.OrderId = orderId;
        }

        /// <summary>
        /// Gets the order Id
        /// </summary>
        public int OrderId
        {
            get;
            private set;
        }
    }
}
