// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;
    using IBApi;

    /// <summary>
    /// Interface for TwsObjectFactory
    /// </summary>
    public interface ITwsObjectFactory
    {
        /// <summary>
        /// Gets the client socket that communicates with TWS
        /// </summary>
        ITwsClientSocket ClientSocket { get; }

        /// <summary>
        /// Gets the EWrapperImplementation for callbacks from TWS
        /// This is exposed only for custom event listeners.
        /// It's not necessary to use this
        /// </summary>
        ITwsCallbackHandler TwsCallbackHandler { get; }

        /// <summary>
        /// Gets the base controller
        /// </summary>
        ITwsControllerBase TwsControllerBase { get; }

        /// <summary>
        /// Gets the Tws controller
        /// </summary>
        ITwsController TwsController { get; }

        /// <summary>
        /// Gets the EWrapper
        /// </summary>
        EWrapper EWrapper { get; }
    }
}