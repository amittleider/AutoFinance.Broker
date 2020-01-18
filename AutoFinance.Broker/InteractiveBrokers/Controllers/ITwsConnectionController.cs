// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System.Threading.Tasks;

    /// <summary>
    /// An interface for the connection controller
    /// </summary>
    public interface ITwsConnectionController
    {
        /// <summary>
        /// Ensures the connection is made
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task EnsureConnectedAsync();

        /// <summary>
        /// Disconnects the session
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DisconnectAsync();
    }
}