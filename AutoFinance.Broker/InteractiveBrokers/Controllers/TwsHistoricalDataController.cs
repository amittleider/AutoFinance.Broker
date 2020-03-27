// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using AutoFinance.Broker.InteractiveBrokers.Exceptions;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;
    using IBApi;

    /// <summary>
    /// This class receives messages about account updates from TWS and stores them.
    /// It provides public methods to retrieve the information on demand.
    /// </summary>
    internal class TwsHistoricalDataController
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
        /// The TWS request Id generator
        /// </summary>
        private ITwsRequestIdGenerator twsRequestIdGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsHistoricalDataController"/> class.
        /// </summary>
        /// <param name="clientSocket">The client socket</param>
        /// <param name="twsCallbackHandler">The callback handler</param>
        /// <param name="twsRequestIdGenerator">The request Id generator</param>
        public TwsHistoricalDataController(ITwsClientSocket clientSocket, ITwsCallbackHandler twsCallbackHandler, ITwsRequestIdGenerator twsRequestIdGenerator)
        {
            this.clientSocket = clientSocket;
            this.twsCallbackHandler = twsCallbackHandler;
            this.twsRequestIdGenerator = twsRequestIdGenerator;
        }

        /// <summary>
        /// Gets historical data from TWS.
        /// </summary>
        /// <param name="contract">The contract type</param>
        /// <param name="endDateTime">The end date of the request</param>
        /// <param name="duration">The duration of the request</param>
        /// <param name="barSizeSetting">The bar size to reuest</param>
        /// <param name="whatToShow">The historical data request type</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<List<HistoricalDataEventArgs>> GetHistoricalDataAsync(Contract contract, DateTime endDateTime, TwsDuration duration, TwsBarSizeSetting barSizeSetting, TwsHistoricalDataRequestType whatToShow)
        {
            int requestId = this.twsRequestIdGenerator.GetNextRequestId();
            int useRth = 1;
            int formatDate = 1;
            List<TagValue> chartOptions = null;

            string value = string.Empty;

            var taskSource = new TaskCompletionSource<List<HistoricalDataEventArgs>>();

            EventHandler<HistoricalDataEventArgs> historicalDataEventHandler = null;
            EventHandler<HistoricalDataEndEventArgs> historicalDataEndEventHandler = null;
            EventHandler<ErrorEventArgs> errorEventHandler = null;

            List<HistoricalDataEventArgs> historicalDataList = new List<HistoricalDataEventArgs>();

            historicalDataEventHandler = (sender, args) =>
            {
                historicalDataList.Add(args);
            };

            historicalDataEndEventHandler = (sender, args) =>
            {
                this.twsCallbackHandler.HistoricalDataEvent -= historicalDataEventHandler;
                this.twsCallbackHandler.HistoricalDataEndEvent -= historicalDataEndEventHandler;
                this.twsCallbackHandler.ErrorEvent -= errorEventHandler;
                taskSource.TrySetResult(historicalDataList);
            };

            errorEventHandler = (sender, args) =>
            {
                if (args.Id == requestId)
                {
                    // The error is associated with this request
                    this.twsCallbackHandler.HistoricalDataEvent -= historicalDataEventHandler;
                    this.twsCallbackHandler.HistoricalDataEndEvent -= historicalDataEndEventHandler;
                    this.twsCallbackHandler.ErrorEvent -= errorEventHandler;
                    taskSource.TrySetException(new TwsException(args.ErrorMessage));
                }
            };

            // Set the operation to cancel after 2 minutes
            CancellationTokenSource tokenSource = new CancellationTokenSource(120 * 1000);
            tokenSource.Token.Register(() =>
            {
                this.twsCallbackHandler.HistoricalDataEvent -= historicalDataEventHandler;
                this.twsCallbackHandler.HistoricalDataEndEvent -= historicalDataEndEventHandler;
                this.twsCallbackHandler.ErrorEvent -= errorEventHandler;

                taskSource.TrySetCanceled();
            });

            this.twsCallbackHandler.HistoricalDataEvent += historicalDataEventHandler;
            this.twsCallbackHandler.HistoricalDataEndEvent += historicalDataEndEventHandler;
            this.twsCallbackHandler.ErrorEvent += errorEventHandler;

            this.clientSocket.ReqHistoricalData(requestId, contract, endDateTime.ToString("yyyyMMdd HH:mm:ss"), duration.ToTwsParameter(), barSizeSetting.ToTwsParameter(), whatToShow.ToTwsParameter(), useRth, formatDate, chartOptions);
            return taskSource.Task;
        }
    }
}
