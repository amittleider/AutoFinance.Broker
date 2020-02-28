// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;

    /// <summary>
    /// This controller handles canceling orders through TWS.
    /// </summary>
    internal class TwsOrderCancelationController : ITwsOrderCancelationController
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
        /// Initializes a new instance of the <see cref="TwsOrderCancelationController"/> class.
        /// </summary>
        /// <param name="clientSocket">The TWS client socket</param>
        /// <param name="twsCallbackHandler">The TWS callback handler</param>
        public TwsOrderCancelationController(ITwsClientSocket clientSocket, ITwsCallbackHandler twsCallbackHandler)
        {
            this.clientSocket = clientSocket;
            this.twsCallbackHandler = twsCallbackHandler;
        }

        /// <summary>
        /// Cancels an order in TWS.
        /// </summary>
        /// <param name="orderId">The order Id to cancel</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. True if the broker acknowledged the cancelation request, false otherwise.</returns>
        public Task<bool> CancelOrderAsync(int orderId)
        {
            var taskSource = new TaskCompletionSource<bool>();

            EventHandler<OrderStatusEventArgs> orderStatusEventCallback = null;
            EventHandler<ErrorEventArgs> errorEventCallback = null;

            orderStatusEventCallback = (sender, eventArgs) =>
            {
                if (eventArgs.OrderId == orderId)
                {
                    if (eventArgs.Status == "Cancelled")
                    {
                        // Unregister the callbacks
                        this.twsCallbackHandler.OrderStatusEvent -= orderStatusEventCallback;

                        // Set the result
                        taskSource.TrySetResult(true);
                    }
                }
            };

            errorEventCallback = (sender, eventArgs) =>
            {
                if (eventArgs.ErrorCode == TwsErrorCodes.OrderCancelled)
                {
                    this.twsCallbackHandler.ErrorEvent -= errorEventCallback;
                    taskSource.TrySetResult(true);
                }
            };

            // Set the operation to cancel after 5 seconds
            CancellationTokenSource tokenSource = new CancellationTokenSource(50000);
            tokenSource.Token.Register(() =>
            {
                taskSource.TrySetCanceled();
            });

            this.twsCallbackHandler.OrderStatusEvent += orderStatusEventCallback;
            this.twsCallbackHandler.ErrorEvent += errorEventCallback;
            this.clientSocket.CancelOrder(orderId);
            return taskSource.Task;
        }
    }
}
