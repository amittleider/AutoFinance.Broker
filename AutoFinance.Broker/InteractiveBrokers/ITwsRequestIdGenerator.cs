// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    /// <summary>
    /// Interface to obtain the next request Id for TWS
    /// </summary>
    internal interface ITwsRequestIdGenerator
    {
        /// <summary>
        /// Get the next request Id
        /// </summary>
        /// <returns>The next valid request Id for TWS</returns>
        int GetNextRequestId();
    }
}
