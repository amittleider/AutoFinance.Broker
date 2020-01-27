// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System.Threading.Tasks;
    using IBApi;

    /// <summary>
    /// Sends orders to TWS
    /// </summary>
    public interface ITwsOrderPlacementController
    {
        /// <summary>
        /// Send an order to TWS
        /// </summary>
        /// <param name="orderId">The order id</param>
        /// <param name="contract">The contract to trade</param>
        /// <param name="order">The order parameters</param>
        /// <returns>True if acknowledged</returns>
        Task<bool> PlaceOrderAsync(int orderId, Contract contract, Order order);
    }
}
