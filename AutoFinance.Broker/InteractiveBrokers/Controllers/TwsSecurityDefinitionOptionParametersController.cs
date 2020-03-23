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
    /// Request option security definitions from TWS.
    /// </summary>
    public class TwsSecurityDefinitionOptionParametersController
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
        /// Initializes a new instance of the <see cref="TwsSecurityDefinitionOptionParametersController"/> class.
        /// </summary>
        /// <param name="clientSocket">The TWS client socket</param>
        /// <param name="twsCallbackHandler">The TWS wrapper implementation</param>
        /// <param name="twsRequestIdGenerator">The TWS request Id generator</param>
        public TwsSecurityDefinitionOptionParametersController(ITwsClientSocket clientSocket, ITwsCallbackHandler twsCallbackHandler, ITwsRequestIdGenerator twsRequestIdGenerator)
        {
            this.clientSocket = clientSocket;
            this.twsCallbackHandler = twsCallbackHandler;
            this.twsRequestIdGenerator = twsRequestIdGenerator;
        }

        /// <summary>
        /// Request security definition parameters.
        /// This is mainly used for finding strikes, multipliers, exchanges, and expirations for options contracts.
        /// </summary>
        /// <param name="underlyingSymbol">The underlying symbol</param>
        /// <param name="exchange">The exchange</param>
        /// <param name="underlyingSecType">The underlying security type</param>
        /// <param name="underlyingConId">The underlying contract ID, retrieved from the Contract Details call</param>
        /// <returns>The security definitions for options</returns>
        public Task<List<SecurityDefinitionOptionParameterEventArgs>> RequestSecurityDefinitionOptionParameters(
            string underlyingSymbol,
            string exchange,
            string underlyingSecType,
            int underlyingConId)
        {
            var taskSource = new TaskCompletionSource<List<SecurityDefinitionOptionParameterEventArgs>>();

            List<SecurityDefinitionOptionParameterEventArgs> securityDefinitionEvents = new List<SecurityDefinitionOptionParameterEventArgs>();

            EventHandler<SecurityDefinitionOptionParameterEventArgs> securityDefinitionEventHandler = null;
            EventHandler securityDefintionEndEventHandler = null;

            securityDefinitionEventHandler = (sender, args) =>
            {
                securityDefinitionEvents.Add(args);
            };

            securityDefintionEndEventHandler = (sender, args) =>
            {
                this.twsCallbackHandler.SecurityDefinitionOptionParameterEvent -= securityDefinitionEventHandler;
                this.twsCallbackHandler.SecurityDefinitionOptionParameterEndEvent -= securityDefintionEndEventHandler;
                taskSource.TrySetResult(securityDefinitionEvents);
            };

            // Set the operation to cancel after 5 seconds
            CancellationTokenSource tokenSource = new CancellationTokenSource(10000);
            tokenSource.Token.Register(() =>
            {
                taskSource.TrySetCanceled();
            });

            this.twsCallbackHandler.SecurityDefinitionOptionParameterEvent += securityDefinitionEventHandler;
            this.twsCallbackHandler.SecurityDefinitionOptionParameterEndEvent += securityDefintionEndEventHandler;

            int requestId = this.twsRequestIdGenerator.GetNextRequestId();
            this.clientSocket.RequestSecurityDefinitionOptionParameters(requestId, underlyingSymbol, exchange, underlyingSecType, underlyingConId);
            return taskSource.Task;
        }
    }
}
