// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// Events container for data from the tickSize TWS endpoint
    /// </summary>
    public class TickReqParamsEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickReqParamsEventArgs"/> class.
        /// </summary>
        /// <param name="tickerId">The ticker id</param>
        /// <param name="minTick">The min tick</param>
        /// <param name="bboExchange">The BBO exchange</param>
        /// <param name="snapshotPermissions">The snapshot permissions</param>
        public TickReqParamsEventArgs(int tickerId, double minTick, string bboExchange, int snapshotPermissions)
        {
            this.TickerId = tickerId;
            this.MinTick = minTick;
            this.BboExchange = bboExchange;
            this.SnapshotPermissions = snapshotPermissions;
        }

        /// <summary>
        /// Gets the ticker id
        /// </summary>
        public int TickerId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the min tick
        /// </summary>
        public double MinTick
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the BBO exchange
        /// </summary>
        public string BboExchange
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the snapshot permissions
        /// </summary>
        public int SnapshotPermissions
        {
            get;
            private set;
        }
    }
}
