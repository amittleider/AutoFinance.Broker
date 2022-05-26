// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers
{
    using System.Threading;
    using AutoFinance.Broker.InteractiveBrokers.Controllers;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;
    using IBApi;

    /// <summary>
    /// Initialize the TWS objects.
    /// </summary>
    public class TwsObjectFactory
    {
        private EReaderMonitorSignal signal;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsObjectFactory"/> class.
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="port">The port</param>
        /// <param name="clientId">The client id</param>
        public TwsObjectFactory(string host, int port, int clientId)
        {
            this.TwsCallbackHandler = new TwsCallbackHandler();

            this.signal = new EReaderMonitorSignal();
            this.ClientSocket = new TwsClientSocket(new EClientSocket(this.TwsCallbackHandler, this.signal));

            this.TwsControllerBase = new TwsControllerBase(this.ClientSocket, this.TwsCallbackHandler, host, port, clientId);
            this.TwsController = new TwsController(this.TwsControllerBase);
        }

        /// <summary>
        /// Gets the client socket that communicates with TWS
        /// </summary>
        public ITwsClientSocket ClientSocket
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the EWrapperImplementation for callbacks from TWS
        /// This is exposed only for custom event listeners.
        /// It's not necessary to use this
        /// </summary>
        public TwsCallbackHandler TwsCallbackHandler
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the base controller
        /// </summary>
        public ITwsControllerBase TwsControllerBase
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Tws controller
        /// </summary>
        public TwsController TwsController
        {
            get;
            private set;
        }
    }
}
