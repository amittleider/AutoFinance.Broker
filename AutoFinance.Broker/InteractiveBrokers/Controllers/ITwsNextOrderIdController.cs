// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
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
    }
}
