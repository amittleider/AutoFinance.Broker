// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;
    using IBApi;

    /// <summary>
    /// This class receives messages about account updates from TWS and stores them.
    /// It provides public methods to retrieve the information on demand.
    /// </summary>
    public class TwsConnectionController
    {
        /// <summary>
        /// The client socket
        /// </summary>
        private ITwsClientSocket clientSocket;

        /// <summary>
        /// The TWS callback handler
        /// </summary>
        private ITwsCallbackHandler twsCallbackHandler;

        /// <summary>
        /// The signal from TWS that says when to read from the queue
        /// </summary>
        private EReaderMonitorSignal signal;
        private string host;
        private int port;
        private int clientId;

        /// <summary>
        /// The background thread that will await the signal and send events to the callback handler
        /// </summary>
        private Thread readerThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsConnectionController"/> class.
        /// Controls the connection to TWS.
        /// </summary>
        /// <param name="clientSocket">The client socket</param>
        /// <param name="twsCallbackHandler">The callback handler</param>
        /// <param name="host">The host name</param>
        /// <param name="port">The port</param>
        /// <param name="clientId">The client ID, see TWS docs</param>
        public TwsConnectionController(
            ITwsClientSocket clientSocket, 
            ITwsCallbackHandler twsCallbackHandler,
            string host,
            int port,
            int clientId)
        {
            this.clientSocket = clientSocket;
            this.twsCallbackHandler = twsCallbackHandler;
            this.signal = new EReaderMonitorSignal();
            this.host = host;
            this.port = port;
            this.clientId = clientId;
        }

        /// <summary>
        /// Connect to the TWS socket and launch a background thread to begin firing the events.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task ConnectAsync()
        {
            var taskSource = new TaskCompletionSource<bool>();

            void connectionAcknowledgementCallback(object sender, EventArgs eventArgs)
            {
                // When the connection is acknowledged, create a reader to consume messages from the TWS.
                // The EReader will consume the incoming messages and the callback handler will begin to fire events.
                this.twsCallbackHandler.ConnectionAcknowledgementEvent -= connectionAcknowledgementCallback;
                var reader = new EReader(this.clientSocket.EClientSocket, this.signal);
                reader.Start();

                this.readerThread = new Thread(
                 () =>
                 {
                     while (true)
                     {
                         this.signal.waitForSignal();
                         reader.processMsgs();
                     }
                 })
                { IsBackground = true };
                this.readerThread.Start();

                taskSource.SetResult(true);
            }

            // Set the operation to cancel after 5 seconds
            ////CancellationTokenSource tokenSource = new CancellationTokenSource(10000);
            ////tokenSource.Token.Register(() =>
            ////{
            ////    taskSource.SetCanceled();
            ////});

            this.twsCallbackHandler.ConnectionAcknowledgementEvent += connectionAcknowledgementCallback;
            this.clientSocket.Connect(this.host, this.port, this.clientId);
            return taskSource.Task;
        }

        /// <summary>
        /// Disconnect from the TWS socket
        /// Note: It appears something is wrong with this API. Need to look into it.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DisconnectAsync()
        {
            var taskSource = new TaskCompletionSource<bool>();
            this.twsCallbackHandler.ConnectionClosedEvent += (sender, eventArgs) =>
            {
                // Abort the reader thread
                taskSource.SetResult(true);
            };

            // Set the operation to cancel after 5 seconds
            CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
            tokenSource.Token.Register(() =>
            {
                taskSource.SetCanceled();
            });

            this.clientSocket.Disconnect();
            return taskSource.Task;
        }
    }
}
