// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;

    /// <summary>
    /// This controller is used to obtain the next valid order Id used in TWS.
    /// </summary>
    public class TwsNextOrderIdController : ITwsNextOrderIdController
    {
        /// <summary>
        /// Holds the state of the next valid order Id for TWS
        /// </summary>
        private static long nextValidOrderId = -1;

        /// <summary>
        /// The client socket
        /// </summary>
        private ITwsClientSocket clientSocket;

        /// <summary>
        /// The TWS callback handler
        /// </summary>
        private ITwsCallbackHandler twsCallbackHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsNextOrderIdController"/> class.
        /// </summary>
        /// <param name="clientSocket">The TWS client socket</param>
        /// <param name="twsCallbackHandler">The TWS callback handler</param>
        public TwsNextOrderIdController(ITwsClientSocket clientSocket, ITwsCallbackHandler twsCallbackHandler)
        {
            this.clientSocket = clientSocket;
            this.twsCallbackHandler = twsCallbackHandler;
            this.twsCallbackHandler.NextValidIdEvent += this.OnNextValidId;
        }

        /// <summary>
        /// Get the next valid order Id.
        /// </summary>
        /// <returns>The next valid order Id</returns>
        public Task<int> GetNextValidIdAsync()
        {
            var taskSource = new TaskCompletionSource<int>();

            long nextId = Interlocked.Read(ref nextValidOrderId);

            if (nextId == -1)
            {
                // The next valid order id has not been sent by TWS yet.
                // It may be taking some time for TWS to initialize or send the event.
                // Wait for it before handling an order status or open order event
                this.twsCallbackHandler.NextValidIdEvent += (sender, eventArgs) =>
                {
                    Interlocked.Exchange(ref nextValidOrderId, eventArgs.OrderId);
                    nextId = Interlocked.Read(ref nextValidOrderId);
                    Interlocked.Increment(ref nextValidOrderId);
                    taskSource.TrySetResult((int)nextId);
                };

                // Set the operation to cancel after 5 seconds
                CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
                tokenSource.Token.Register(() =>
                {
                    taskSource.TrySetCanceled();
                });
            }
            else
            {
                // The next valid Id has already been assigned. Increment it and return.
                Interlocked.Increment(ref nextValidOrderId);
                taskSource.TrySetResult((int)nextId);
            }

            return taskSource.Task;
        }

        /// <summary>
        /// This event is set during the initialization of the object.
        /// This event handler should be called only once during the startup of Tws.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="eventArgs">The event arguments</param>
        private void OnNextValidId(object sender, NextValidIdEventArgs eventArgs)
        {
            long orderId = 0;
            Interlocked.Exchange(ref nextValidOrderId, eventArgs.OrderId);
            orderId = Interlocked.Read(ref nextValidOrderId);
            Interlocked.Increment(ref nextValidOrderId);

            // Unsubscribe the main event
            this.twsCallbackHandler.NextValidIdEvent -= this.OnNextValidId;
        }
    }
}
