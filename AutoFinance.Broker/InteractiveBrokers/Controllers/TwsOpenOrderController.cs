// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;
    using IBApi;

    /// <summary>
    /// Exposes APIs to place orders through TWS
    /// </summary>
    public class TwsOpenOrderController //: ITwsOpenOrderController
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
        /// The connection controller
        /// </summary>
        private TwsConnectionController connectionController;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsOrderPlacementController"/> class.
        /// </summary>
        /// <param name="clientSocket">The TWS client socket</param>
        /// <param name="twsCallbackHandler">The TWS wrapper implementation</param>
        public TwsOpenOrderController(ITwsClientSocket clientSocket, TwsCallbackHandler twsCallbackHandler)
        {
            this.clientSocket = clientSocket;
            this.twsCallbackHandler = twsCallbackHandler;
        }

        public Task<ConcurrentBag<OpenOrderEventArgs>> GetOpenOrders()
        {
            var taskSource = new TaskCompletionSource<ConcurrentBag<OpenOrderEventArgs>>();
            ConcurrentBag<OpenOrderEventArgs> symbols = new ConcurrentBag<OpenOrderEventArgs>();

            EventHandler<OpenOrderEventArgs> openOrderEventCallback = null;
            openOrderEventCallback = (sender, eventArgs) =>
            {
                symbols.Add(eventArgs);
            };

            EventHandler<OpenOrderEndEventArgs> openOrderEndEventCallback = null;
            openOrderEndEventCallback = (sender, eventArgs) =>
            {
                // Unsubscribe from the event handlers
                this.twsCallbackHandler.OpenOrderEvent -= openOrderEventCallback;
                this.twsCallbackHandler.OpenOrderEndEvent -= openOrderEndEventCallback;
                taskSource.TrySetResult(symbols);
            };

            // Set the operation to cancel after 5 seconds
            CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
            tokenSource.Token.Register(() =>
            {
                taskSource.TrySetCanceled();
            });

            this.twsCallbackHandler.OpenOrderEndEvent += openOrderEndEventCallback;
            this.twsCallbackHandler.OpenOrderEvent += openOrderEventCallback;

            this.clientSocket.RequestOpenOrders();
            return taskSource.Task;
        }
    }
}
