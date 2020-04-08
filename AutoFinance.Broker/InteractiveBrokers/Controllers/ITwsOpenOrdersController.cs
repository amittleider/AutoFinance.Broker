// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;

    /// <summary>
    /// An interface for the open orders controller
    /// </summary>
    public interface ITwsOpenOrdersController
    {
        /// <summary>
        /// Requests open orders
        /// </summary>
        /// <returns>Open orders</returns>
        Task<List<OpenOrderEventArgs>> RequestOpenOrders();

        /// <summary>
        /// Requests open orders
        /// </summary>
        /// <param name="cancellationToken">The cancellation token used to cancel the request</param>
        /// <returns>Open orders</returns>
        Task<List<OpenOrderEventArgs>> RequestOpenOrders(CancellationToken cancellationToken);
    }
}
