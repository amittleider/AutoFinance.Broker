// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Controllers;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
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
        /// Stores the results of contract details responses from IB.
        /// </summary>
        private ConcurrentDictionary<int, ContractDetails> contractDetailsPerRequest;

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
            this.contractDetailsPerRequest = new ConcurrentDictionary<int, ContractDetails>();
            this.twsCallbackHandler.ContractDetailsEvent += this.OnContractDetailsEvent;
            this.twsCallbackHandler.ContractDetailsEndEvent += this.OnContractDetailsEndEvent;
        }

        /// <summary>
        /// Gets a contract by request.
        /// </summary>
        /// <param name="contract">The requested contract.</param>
        /// <returns>The details of the contract</returns>
        public Task<ContractDetails> GetContractAsync(Contract contract)
        {
            int requestId = this.twsRequestIdGenerator.GetNextRequestId();
            var taskSource = new TaskCompletionSource<ContractDetails>();
            this.twsCallbackHandler.ContractDetailsEndEvent += (sender, args) =>
            {
                // When the contract details end event is fired, check if it's for this request ID and return it.
                taskSource.TrySetResult(this.contractDetailsPerRequest[requestId]);
            };

            // Set the operation to cancel after 5 seconds
            CancellationTokenSource tokenSource = new CancellationTokenSource(5000);
            tokenSource.Token.Register(() =>
            {
                taskSource.TrySetCanceled();
            });

            this.clientSocket.ReqContractDetails(requestId, contract);
            return taskSource.Task;
        }

        /// <summary>
        /// This event fires when a new contract details event is raised fromt he TWS callback handler.
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="contractDetailsEventArgs">The event arguments</param>
        private void OnContractDetailsEvent(object sender, ContractDetailsEventArgs contractDetailsEventArgs)
        {
            this.contractDetailsPerRequest.AddOrUpdate(contractDetailsEventArgs.RequestId, contractDetailsEventArgs.ContractDetails, this.UpdateContractDetailsPerRequest);
        }

        /// <summary>
        /// Update the value of the concurrent dictionary with the contract details.
        /// </summary>
        /// <param name="requestId">The request ID</param>
        /// <param name="contractDetails">The contract details</param>
        /// <returns>The contract details of the input. We don't actually care about previous contract details values.</returns>
        private ContractDetails UpdateContractDetailsPerRequest(int requestId, ContractDetails contractDetails)
        {
            return contractDetails;
        }

        /// <summary>
        /// Actually I think we don't care about this event as long as we assume that there will only be one contract in the response.
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="contractDetailsEndEventArgs">The event args to end the request.</param>
        private void OnContractDetailsEndEvent(object sender, ContractDetailsEndEventArgs contractDetailsEndEventArgs)
        {
        }
    }
}
