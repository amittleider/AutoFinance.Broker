// Licensed under the Apache License, Version 2.0.

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
    public class TwsConnectionController : ITwsConnectionController
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
        private bool connected;

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
            this.connected = false;
        }

        /// <summary>
        /// Ensures the connection is active
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task EnsureConnectedAsync()
        {
            if (!this.connected)
            {
                await this.ConnectAsync();
                this.connected = true;
            }
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
                taskSource.TrySetResult(true);
            };

            // Set the operation to cancel after 5 seconds
            CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
            tokenSource.Token.Register(() =>
            {
                taskSource.TrySetCanceled();
            });

            this.clientSocket.Disconnect();
            return taskSource.Task;
        }

        /// <summary>
        /// Connect to the TWS socket and launch a background thread to begin firing the events.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private Task ConnectAsync()
        {
            var taskSource = new TaskCompletionSource<bool>();

            void ConnectionAcknowledgementCallback(object sender, EventArgs eventArgs)
            {
                // When the connection is acknowledged, create a reader to consume messages from the TWS.
                // The EReader will consume the incoming messages and the callback handler will begin to fire events.
                this.twsCallbackHandler.ConnectionAcknowledgementEvent -= ConnectionAcknowledgementCallback;
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

                taskSource.TrySetResult(true);
            }

            // Set the operation to cancel after 5 seconds
            CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
            tokenSource.Token.Register(() =>
            {
               taskSource.TrySetCanceled();
            });

            this.twsCallbackHandler.ConnectionAcknowledgementEvent += ConnectionAcknowledgementCallback;
            this.clientSocket.Connect(this.host, this.port, this.clientId);
            return taskSource.Task;
        }
    }
}
