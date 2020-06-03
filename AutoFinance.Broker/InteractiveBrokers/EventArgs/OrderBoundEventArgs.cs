// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The event arguments when a order is bound through the API.
    /// </summary>
    public class OrderBoundEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderBoundEventArgs"/> class.
        /// </summary>
        /// <param name="orderId">The permanent id.</param>
        /// <param name="apiClientId">The API client id.</param>
        /// <param name="apiOrderId">The API order id.</param>
        public OrderBoundEventArgs(long orderId, int apiClientId, int apiOrderId)
       {
           this.OrderId = orderId;
           this.ApiClientId = apiClientId;
           this.ApiOrderId = apiOrderId;
       }

        /// <summary>
        /// Gets the request id.
        /// </summary>
        public long OrderId { get; private set; }

        /// <summary>
        /// Gets the Api client id.
        /// </summary>
        public int ApiClientId { get; private set; }

        /// <summary>
        /// Gets the Api order id.
        /// </summary>
        public int ApiOrderId { get; private set; }
    }
}
