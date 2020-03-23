// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Controllers;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using AutoFinance.Broker.InteractiveBrokers.Exceptions;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;
    using IBApi;

    /// <summary>
    /// This class receives messages from TWS regarding contract details and stores them.
    /// It then exposes an API to retreive them on demand.
    /// </summary>
    public class TwsContractDetailsController
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
        /// The request Id generator
        /// </summary>
        private ITwsRequestIdGenerator twsRequestIdGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsContractDetailsController"/> class.
        /// The contract details controller exposes methods to retrieve contract details on-demand instead of through a subscription.
        /// </summary>
        /// <param name="clientSocket">The TWS client socket</param>
        /// <param name="twsCallbackHandler">The TWS wrapper implementation</param>
        /// <param name="twsRequestIdGenerator">The TWS request Id generator</param>
        public TwsContractDetailsController(ITwsClientSocket clientSocket, ITwsCallbackHandler twsCallbackHandler, ITwsRequestIdGenerator twsRequestIdGenerator)
        {
            this.clientSocket = clientSocket;
            this.twsCallbackHandler = twsCallbackHandler;
            this.twsRequestIdGenerator = twsRequestIdGenerator;
        }

        /// <summary>
        /// Gets a contract by request.
        /// </summary>
        /// <param name="contract">The requested contract.</param>
        /// <returns>The details of the contract</returns>
        public Task<List<ContractDetails>> GetContractAsync(Contract contract)
        {
            int requestId = this.twsRequestIdGenerator.GetNextRequestId();
            List<ContractDetails> contractDetailsList = new List<ContractDetails>();

            var taskSource = new TaskCompletionSource<List<ContractDetails>>();

            EventHandler<ContractDetailsEventArgs> contractDetailsEventHandler = null;
            EventHandler<ContractDetailsEndEventArgs> contractDetailsEndEventHandler = null;
            EventHandler<ErrorEventArgs> errorEventHandler = null;

            contractDetailsEventHandler += (sender, args) =>
            {
                // When the contract details end event is fired, check if it's for this request ID and return it.
                contractDetailsList.Add(args.ContractDetails);
            };

            contractDetailsEndEventHandler += (sender, args) =>
            {
                taskSource.TrySetResult(contractDetailsList);
            };

            errorEventHandler = (sender, args) =>
            {
                if (args.Id == requestId)
                {
                    // The error is associated with this request
                    this.twsCallbackHandler.ContractDetailsEvent -= contractDetailsEventHandler;
                    this.twsCallbackHandler.ContractDetailsEndEvent -= contractDetailsEndEventHandler;
                    this.twsCallbackHandler.ErrorEvent -= errorEventHandler;
                    taskSource.TrySetException(new TwsException(args.ErrorMessage));
                }
            };

            // Set the operation to cancel after 5 seconds
            CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
            tokenSource.Token.Register(() =>
            {
                taskSource.TrySetCanceled();
            });

            this.twsCallbackHandler.ContractDetailsEvent += contractDetailsEventHandler;
            this.twsCallbackHandler.ContractDetailsEndEvent += contractDetailsEndEventHandler;
            this.twsCallbackHandler.ErrorEvent += errorEventHandler;

            this.clientSocket.ReqContractDetails(requestId, contract);
            return taskSource.Task;
        }
    }
}
