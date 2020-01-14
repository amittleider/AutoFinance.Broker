// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    using IBApi;

    /// <summary>
    /// The event arguments for an OpenOrder event raised by TWS
    /// </summary>
    public class OpenOrderEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenOrderEventArgs"/> class.
        /// </summary>
        /// <param name="orderId">The order Id</param>
        /// <param name="contract">The contract</param>
        /// <param name="order">The order</param>
        /// <param name="orderState">The order state</param>
        public OpenOrderEventArgs(int orderId, Contract contract, Order order, OrderState orderState)
        {
            this.OrderId = orderId;
            this.Contract = contract;
            this.Order = order;
            this.OrderState = orderState;
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
        /// Gets the contract
        /// </summary>
        public Contract Contract
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the order
        /// </summary>
        public Order Order
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the order state
        /// </summary>
        public OrderState OrderState
        {
            get;
            private set;
        }
    }
}
