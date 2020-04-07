// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// An interface for retrieving the next valid Order Id for TWS
    /// </summary>
    public interface ITwsNextOrderIdController
    {
        /// <summary>
        /// Gets the next valid order Id for TWS
        /// </summary>
        /// <returns>The next valid order Id</returns>
        Task<int> GetNextValidIdAsync();

        /// <summary>
        /// Gets the next valid order Id for TWS
        /// </summary>
        /// <param name="cancellationToken">The cancellation token used to cancel the request</param>
        /// <returns>The next valid order Id</returns>
        Task<int> GetNextValidIdAsync(CancellationToken cancellationToken);
    }
}
