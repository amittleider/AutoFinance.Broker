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
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;
    using IBApi;

    /// <summary>
    /// The controller at the level that directly wraps TWS methods
    /// </summary>
    public interface ITwsControllerBase
    {
        /// <summary>
        /// Gets a value indicating whether is the client connected to tws
        /// </summary>
        bool Connected
        {
            get;
        }

        /// <summary>
        /// Ensures the connection is made
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task EnsureConnectedAsync();

        /// <summary>
        /// Disconnects the session
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DisconnectAsync();

        /// <summary>
        /// Gets the next valid order Id for TWS
        /// </summary>
        /// <returns>The next valid order Id</returns>
        Task<int> GetNextValidIdAsync();

        /// <summary>
        /// Gets the next valid order Id for TWS
        /// </summary>
        /// <param name="cancellationToken">The cancellation token used to cancel the request</param>
        /// <returns>The next valid order Id</returns>
        Task<int> GetNextValidIdAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the next request id.
        /// </summary>
        /// <returns>The next request id</returns>
        int GetNextRequestId();

        /// <summary>
        /// Requests open orders
        /// </summary>
        /// <returns>Open orders</returns>
        Task<List<OpenOrderEventArgs>> RequestOpenOrders();

        /// <summary>
        /// Requests open orders
        /// </summary>
        /// <param name="cancellationToken">The cancellation token used to cancel the request</param>
        /// <returns>Open orders</returns>
        Task<List<OpenOrderEventArgs>> RequestOpenOrders(CancellationToken cancellationToken);

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>True if it was successfully cancelled</returns>
        Task<bool> CancelOrderAsync(int orderId);

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the request</param>
        /// <returns>True if it was successfully cancelled</returns>
        Task<bool> CancelOrderAsync(int orderId, CancellationToken cancellationToken);

        /// <summary>
        /// Send an order to TWS
        /// </summary>
        /// <param name="orderId">The order id</param>
        /// <param name="contract">The contract to trade</param>
        /// <param name="order">The order parameters</param>
        /// <returns>True if acknowledged</returns>
        Task<bool> PlaceOrderAsync(int orderId, Contract contract, Order order);

        /// <summary>
        /// Send an order to TWS
        /// </summary>
        /// <param name="orderId">The order id</param>
        /// <param name="contract">The contract to trade</param>
        /// <param name="order">The order parameters</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the request</param>
        /// <returns>True if acknowledged</returns>
        Task<bool> PlaceOrderAsync(int orderId, Contract contract, Order order, CancellationToken cancellationToken);

        /// <summary>
        /// Cancel account detail update
        /// </summary>
        /// <param name="accountId">The account Id</param>
        void CancelAccountDetails(string accountId);

        /// <summary>
        /// Get an account detail
        /// </summary>
        /// <param name="accountId">The account Id</param>
        /// <returns>The account values</returns>
        Task<ConcurrentDictionary<string, string>> GetAccountDetailsAsync(string accountId);

        /// <summary>
        /// Get an account detail
        /// </summary>
        /// <param name="accountId">The account Id</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the request</param>
        /// <returns>The account values</returns>
        Task<ConcurrentDictionary<string, string>> GetAccountDetailsAsync(string accountId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a contract by request.
        /// </summary>
        /// <param name="contract">The requested contract.</param>
        /// <returns>The details of the contract</returns>
        Task<List<ContractDetails>> GetContractAsync(Contract contract);

        /// <summary>
        /// Get a list of all the executions from TWS
        /// </summary>
        /// <returns>A list of execution details events from TWS.</returns>
        Task<List<ExecutionDetailsEventArgs>> RequestExecutions();

        /// <summary>
        /// Gets historical data from TWS.
        /// </summary>
        /// <param name="contract">The contract type</param>
        /// <param name="endDateTime">The end date of the request</param>
        /// <param name="duration">The duration of the request</param>
        /// <param name="barSizeSetting">The bar size to request</param>
        /// <param name="whatToShow">The historical data request type</param>
        /// <param name="useRth">Whether to use regular trading hours</param>
        /// <param name="formatDate">Whether to format date</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<HistoricalDataEventArgs>> GetHistoricalDataAsync(
            Contract contract,
            DateTime endDateTime,
            TwsDuration duration,
            TwsBarSizeSetting barSizeSetting,
            TwsHistoricalDataRequestType whatToShow,
            bool useRth = true,
            bool formatDate = true);

        /// <summary>
        /// Gets historical data from TWS.
        /// </summary>
        /// <param name="contract">The contract type</param>
        /// <param name="endDateTime">The end date of the request</param>
        /// <param name="duration">The duration of the request</param>
        /// <param name="barSizeSetting">The bar size to request</param>
        /// <param name="whatToShow">The historical data request type</param>
        /// <param name="useRth">Whether to use regular trading hours</param>
        /// <param name="formatDate">Whether to format date</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<HistoricalDataEventArgs>> GetHistoricalDataAsync(
            Contract contract,
            DateTime endDateTime,
            string duration,
            string barSizeSetting,
            string whatToShow,
            int useRth,
            int formatDate);

        /// <summary>
        /// Request Historical Data cancelation
        /// </summary>
        /// <param name="requestId">The request Id</param>
        void CancelHistoricalData(int requestId);

        /// <summary>
        /// Gets news providers from TWS
        /// </summary>
        /// <returns>News providers</returns>
        Task<NewsProviderEventArgs> GetNewsProviders();

        /// <summary>
        /// Get a list of all the positions in TWS.
        /// </summary>
        /// <returns>A list of position status events from TWS.</returns>
        Task<List<PositionStatusEventArgs>> RequestPositions();

        /// <summary>
        /// Sends a message to TWS telling it to stop sending position information through the socket.
        /// </summary>
        void CancelPositions();

        /// <summary>
        /// Request security definition parameters.
        /// This is mainly used for finding strikes, multipliers, exchanges, and expirations for options contracts.
        /// </summary>
        /// <param name="underlyingSymbol">The underlying symbol</param>
        /// <param name="exchange">The exchange</param>
        /// <param name="underlyingSecType">The underlying security type</param>
        /// <param name="underlyingConId">The underlying contract ID, retrieved from the Contract Details call</param>
        /// <returns>The security definitions for options</returns>
        Task<List<SecurityDefinitionOptionParameterEventArgs>> RequestSecurityDefinitionOptionParameters(
            string underlyingSymbol,
            string exchange,
            string underlyingSecType,
            int underlyingConId);

        /// <summary>
        /// Request market data from TWS.
        /// </summary>
        /// <param name="contract">The contract type</param>
        /// <param name="genericTickList">The generic tick list</param>
        /// <param name="snapshot">The snapshot flag</param>
        /// <param name="regulatorySnapshot">The regulatory snapshot flag</param>
        /// <param name="mktDataOptions">The market data options</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<TickSnapshotEndEventArgs> RequestMarketDataAsync(
            Contract contract,
            string genericTickList,
            bool snapshot,
            bool regulatorySnapshot,
            List<TagValue> mktDataOptions);

        /// <summary>
        /// Cancel market data
        /// </summary>
        /// <param name="requestId">The request to cancel</param>
        void CancelMarketData(int requestId);

        /// <summary>
        /// Request real time bars data from TWS.
        /// </summary>
        /// <param name="contract">The contract type</param>
        /// <param name="barSize">The bar size</param>
        /// <param name="whatToShow">The whatToShow parameters</param>
        /// <param name="useRTH">The regular time flag</param>
        /// <param name="realTimeBarsOptions">The real time bars options</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<RealtimeBarEventArgs> RequestRealtimeBarAsync(
            Contract contract,
            int barSize,
            string whatToShow,
            bool useRTH,
            List<TagValue> realTimeBarsOptions);

        /// <summary>
        /// Cancel real time bars data
        /// </summary>
        /// <param name="requestId">The request to cancel</param>
        void CancelRealtimeBars(int requestId);

        /// <summary>
        /// Set the type for the market data feed
        /// </summary>
        /// <param name="marketDataTypeId">The feed level</param>
        void RequestMarketDataType(int marketDataTypeId);

        /// <summary>
        /// Get the PnL of the account.
        /// </summary>
        /// <param name="accountCode">The account code</param>
        /// <param name="modelCode">The model code</param>
        /// <returns>The PnL account update event from TWS.</returns>
        Task<PnLEventArgs> RequestPnL(
            string accountCode,
            string modelCode);

        /// <summary>
        /// Request PnL update cancelation
        /// </summary>
        /// <param name="requestId">The request Id</param>
        void CancelPnL(int requestId);

        /// <summary>
        /// Get the PnL of the account.
        /// </summary>
        /// <param name="accountCode">The account code</param>
        /// <param name="modelCode">The model code</param>
        /// <param name="conId">The contract Id</param>
        /// <returns>The single position PnL update event from TWS.</returns>
        Task<PnLSingleEventArgs> RequestPnLSingle(
            string accountCode,
            string modelCode,
            int conId);

        /// <summary>
        /// Request PnL update cancelation
        /// </summary>
        /// <param name="requestId">The request Id</param>
        void CancelPnLSingle(int requestId);
    }
}
