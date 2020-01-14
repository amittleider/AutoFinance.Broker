// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers
{
    using System.Threading;
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
        public TwsObjectFactory()
        {
            this.TwsCallbackHandler = new TwsCallbackHandler();

            this.signal = new EReaderMonitorSignal();
            this.ClientSocket = new TwsClientSocket(new EClientSocket(this.TwsCallbackHandler, this.signal));
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
        /// </summary>
        public TwsCallbackHandler TwsCallbackHandler
        {
            get;
            private set;
        }
    }
}
