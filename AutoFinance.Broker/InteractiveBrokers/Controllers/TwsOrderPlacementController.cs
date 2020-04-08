// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;
    using IBApi;

    /// <summary>
    /// Exposes APIs to place orders through TWS
    /// </summary>
    public class TwsOrderPlacementController : ITwsOrderPlacementController
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
        /// Initializes a new instance of the <see cref="TwsOrderPlacementController"/> class.
        /// </summary>
        /// <param name="clientSocket">The TWS client socket</param>
        /// <param name="twsCallbackHandler">The TWS wrapper implementation</param>
        public TwsOrderPlacementController(ITwsClientSocket clientSocket, ITwsCallbackHandler twsCallbackHandler)
        {
            this.clientSocket = clientSocket;
            this.twsCallbackHandler = twsCallbackHandler;
        }

        /// <summary>
        /// Places an order and returns whether the order placement was successful or not.
        /// </summary>
        /// <param name="orderId">The order Id</param>
        /// <param name="contract">The contract to trade</param>
        /// <param name="order">The order</param>
        /// <returns>True if the order was acknowledged, false otherwise</returns>
        public Task<bool> PlaceOrderAsync(int orderId, Contract contract, Order order)
        {
            // Set the operation to cancel after 5 seconds
            CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
            return this.PlaceOrderAsync(orderId, contract, order, tokenSource.Token);
        }

        /// <summary>
        /// Places an order and returns whether the order placement was successful or not.
        /// </summary>
        /// <param name="orderId">The order Id</param>
        /// <param name="contract">The contract to trade</param>
        /// <param name="order">The order</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the request</param>
        /// <returns>True if the order was acknowledged, false otherwise</returns>
        public Task<bool> PlaceOrderAsync(int orderId, Contract contract, Order order, CancellationToken cancellationToken)
        {
            var taskSource = new TaskCompletionSource<bool>();

            EventHandler<OpenOrderEventArgs> openOrderEventCallback = null;

            openOrderEventCallback = (sender, eventArgs) =>
            {
                if (eventArgs.OrderId == orderId)
                {
                    if (eventArgs.OrderState.Status == TwsOrderStatus.Submitted ||
                        eventArgs.OrderState.Status == TwsOrderStatus.Presubmitted)
                    {
                        // Unregister the callbacks
                        this.twsCallbackHandler.OpenOrderEvent -= openOrderEventCallback;

                        taskSource.TrySetResult(true);
                    }
                }
            };

            EventHandler<ErrorEventArgs> orderErrorEventCallback = null;
            orderErrorEventCallback = (sender, eventArgs) =>
            {
                if (orderId == eventArgs.Id)
                {
                    if (eventArgs.ErrorCode == TwsErrorCodes.InvalidOrderType ||
                        eventArgs.ErrorCode == TwsErrorCodes.AmbiguousContract)
                    {
                        // Unregister the callbacks
                        this.twsCallbackHandler.OpenOrderEvent -= openOrderEventCallback;
                        this.twsCallbackHandler.ErrorEvent -= orderErrorEventCallback;
                        taskSource.TrySetResult(false);
                    }
                }
            };

            this.twsCallbackHandler.ErrorEvent += orderErrorEventCallback;
            this.twsCallbackHandler.OpenOrderEvent += openOrderEventCallback;

            cancellationToken.Register(() =>
            {
                taskSource.TrySetCanceled();
            });

            // TODO: Implement the error handler here as well
            this.clientSocket.PlaceOrder(orderId, contract, order);
            return taskSource.Task;
        }
    }
}
