// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System.Collections.Generic;
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
    }
}
