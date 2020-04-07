// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// An interfact to cancel orders in TWS
    /// </summary>
    public interface ITwsOrderCancelationController
    {
        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>True if it was successfully cancelled</returns>
        Task<bool> CancelOrderAsync(int orderId);

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the request</param>
        /// <returns>True if it was successfully cancelled</returns>
        Task<bool> CancelOrderAsync(int orderId, CancellationToken cancellationToken);
    }
}
