// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    using IBApi;

    /// <summary>
    /// The event arguments when completed order is fetched.
    /// </summary>
    public class CompletedOrderEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompletedOrderEventArgs"/> class.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="order">The order completed.</param>
        /// <param name="orderState">The order state.</param>
        public CompletedOrderEventArgs(Contract contract, Order order, OrderState orderState)
       {
           this.Contract = contract;
           this.Order = order;
           this.OrderState = orderState;
       }

        /// <summary>
        /// Gets the contract
        /// </summary>
        public Contract Contract { get; private set; }

        /// <summary>
        /// Gets the order
        /// </summary>
        public Order Order{ get; private set; }

        /// <summary>
        /// Gets the order state
        /// </summary>
        public OrderState OrderState { get; private set; }
    }
}
