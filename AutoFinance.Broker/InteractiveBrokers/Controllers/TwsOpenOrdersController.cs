// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;

    /// <summary>
    /// Get open orders from TWS
    /// </summary>
    public class TwsOpenOrdersController : ITwsOpenOrdersController
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
        /// Initializes a new instance of the <see cref="TwsOpenOrdersController"/> class.
        /// </summary>
        /// <param name="clientSocket">The TWS client socket</param>
        /// <param name="twsCallbackHandler">The TWS callback handler</param>
        public TwsOpenOrdersController(ITwsClientSocket clientSocket, ITwsCallbackHandler twsCallbackHandler)
        {
            this.clientSocket = clientSocket;
            this.twsCallbackHandler = twsCallbackHandler;
        }

        /// <summary>
        /// Get a list of all the executions from TWS
        /// </summary>
        /// <returns>A list of execution details events from TWS.</returns>
        public Task<List<OpenOrderEventArgs>> RequestOpenOrders()
        {
            // Set the operation to cancel after 5 seconds
            CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
            return this.RequestOpenOrders(tokenSource.Token);
        }

        /// <summary>
        /// Get a list of all the executions from TWS
        /// </summary>
        /// <param name="cancellationToken">The cancellation token used to cancel the request</param>
        /// <returns>A list of execution details events from TWS.</returns>
        public Task<List<OpenOrderEventArgs>> RequestOpenOrders(CancellationToken cancellationToken)
        {
            List<OpenOrderEventArgs> openOrderEvents = new List<OpenOrderEventArgs>();
            var taskSource = new TaskCompletionSource<List<OpenOrderEventArgs>>();
            EventHandler<OpenOrderEventArgs> openOrderEventHandler = null;
            EventHandler<OpenOrderEndEventArgs> openOrderEndEventHandler = null;

            openOrderEventHandler = (sender, args) =>
            {
                openOrderEvents.Add(args);
            };

            openOrderEndEventHandler = (sender, args) =>
            {
                // Cleanup the event handlers when the positions end event is called.
                this.twsCallbackHandler.OpenOrderEvent -= openOrderEventHandler;
                this.twsCallbackHandler.OpenOrderEndEvent -= openOrderEndEventHandler;

                taskSource.TrySetResult(openOrderEvents);
            };

            this.twsCallbackHandler.OpenOrderEvent += openOrderEventHandler;
            this.twsCallbackHandler.OpenOrderEndEvent += openOrderEndEventHandler;

            cancellationToken.Register(() =>
            {
                taskSource.TrySetCanceled();
            });

            this.clientSocket.RequestAllOpenOrders();

            return taskSource.Task;
        }
    }
}
