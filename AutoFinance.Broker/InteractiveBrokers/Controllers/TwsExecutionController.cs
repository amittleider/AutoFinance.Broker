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
    /// This class receives messages about account updates from TWS and stores them.
    /// It provides public methods to retrieve the information on demand.
    /// </summary>
    internal class TwsExecutionController
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
        /// The TWS request ID generator
        /// </summary>
        private ITwsRequestIdGenerator twsRequestIdGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsExecutionController"/> class.
        /// </summary>
        /// <param name="clientSocket">The TWS client socket</param>
        /// <param name="twsCallbackHandler">The TWS callback handler</param>
        /// <param name="twsRequestIdGenerator">The TWS request ID generator</param>
        public TwsExecutionController(ITwsClientSocket clientSocket, ITwsCallbackHandler twsCallbackHandler, ITwsRequestIdGenerator twsRequestIdGenerator)
        {
            this.clientSocket = clientSocket;
            this.twsCallbackHandler = twsCallbackHandler;
            this.twsRequestIdGenerator = twsRequestIdGenerator;
        }

        /// <summary>
        /// Get a list of all the executions from TWS
        /// </summary>
        /// <returns>A list of execution details events from TWS.</returns>
        public Task<List<ExecutionDetailsEventArgs>> RequestExecutions()
        {
            // Set the operation to cancel after 5 seconds
            CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
            return this.RequestExecutions(tokenSource.Token);
        }

        /// <summary>
        /// Get a list of all the executions from TWS
        /// </summary>
        /// <param name="cancellationToken">The cancellation token used to cancel the request</param>
        /// <returns>A list of execution details events from TWS.</returns>
        public Task<List<ExecutionDetailsEventArgs>> RequestExecutions(CancellationToken cancellationToken)
        {
            List<ExecutionDetailsEventArgs> executionDetailsEvents = new List<ExecutionDetailsEventArgs>();
            var taskSource = new TaskCompletionSource<List<ExecutionDetailsEventArgs>>();
            EventHandler<ExecutionDetailsEventArgs> executionDetailsEvent = null;
            EventHandler<ExecutionDetailsEndEventArgs> executionDetailsEndEvent = null;

            executionDetailsEvent = (sender, args) =>
            {
                executionDetailsEvents.Add(args);
            };

            executionDetailsEndEvent = (sender, args) =>
            {
                // Cleanup the event handlers when the positions end event is called.
                this.twsCallbackHandler.ExecutionDetailsEvent -= executionDetailsEvent;
                this.twsCallbackHandler.ExecutionDetailsEndEvent -= executionDetailsEndEvent;

                taskSource.TrySetResult(executionDetailsEvents);
            };

            this.twsCallbackHandler.ExecutionDetailsEvent += executionDetailsEvent;
            this.twsCallbackHandler.ExecutionDetailsEndEvent += executionDetailsEndEvent;

            cancellationToken.Register(() =>
            {
                taskSource.TrySetCanceled();
            });

            this.clientSocket.RequestExecutions(this.twsRequestIdGenerator.GetNextRequestId());

            return taskSource.Task;
        }
    }
}
