// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// Events container for data from the tickSnapshotEnd TWS endpoint
    /// </summary>
    public class TickSnapshotEndEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickSnapshotEndEventArgs"/> class.
        /// </summary>
        /// <param name="tickerId">The ticker id</param>
        public TickSnapshotEndEventArgs(int tickerId)
        {
            this.TickerId = tickerId;
        }

        /// <summary>
        /// Gets the ticker id
        /// </summary>
        public int TickerId
        {
            get;
            private set;
        }
    }
}
