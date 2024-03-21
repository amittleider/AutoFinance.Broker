// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// Provides event arguments from order status events sent from TWS
    /// </summary>
    public class OrderStatusEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderStatusEventArgs"/> class.
        /// The order status event arguments for the order status callback of TWS.
        /// </summary>
        /// <param name="orderId">The order Id</param>
        /// <param name="status">The order status</param>
        /// <param name="filled">The number of filled shares</param>
        /// <param name="remaining">The number of remaining shrares</param>
        /// <param name="avgFillPrice">The average fill price</param>
        /// <param name="permId">The perm Id (?)</param>
        /// <param name="parentId">The parent Id (?)</param>
        /// <param name="lastFillPrice">The last fill price of the order</param>
        /// <param name="clientId">The client Id that sent the order. The client Id should be static in this application.</param>
        /// <param name="whyHeld">Why held (?)</param>
        public OrderStatusEventArgs(int orderId, string status, decimal filled, decimal remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld)
        {
            this.OrderId = orderId;
            this.Status = status;
            this.Filled = filled;
            this.Remaining = remaining;
            this.AvgFillPrice = avgFillPrice;
            this.PermId = permId;
            this.ParentId = parentId;
            this.LastFillPrice = lastFillPrice;
            this.ClientId = clientId;
            this.WhyHeld = whyHeld;
        }

        /// <summary>
        /// Gets the order Id
        /// </summary>
        public int OrderId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the order status
        /// </summary>
        public string Status
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the amount of shares filled
        /// </summary>
        public decimal Filled
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the amount of shares remaining
        /// </summary>
        public decimal Remaining
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the average fill price
        /// </summary>
        public double AvgFillPrice
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets not sure
        /// </summary>
        public int PermId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets not sure
        /// </summary>
        public int ParentId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the last fill price of this order
        /// </summary>
        public double LastFillPrice
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the client Id that sent the order. It should be constant in this application
        /// </summary>
        public int ClientId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets not sure
        /// </summary>
        public string WhyHeld
        {
            get;
            private set;
        }
    }
}
