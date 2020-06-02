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
        /// Ensures the connection is made
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task EnsureConnectedAsync();

        bool IsConnected();

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
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<HistoricalDataEventArgs>> GetHistoricalDataAsync(Contract contract, DateTime endDateTime, TwsDuration duration, TwsBarSizeSetting barSizeSetting, TwsHistoricalDataRequestType whatToShow);

        /// <summary>
        /// Gets historical data from TWS.
        /// </summary>
        /// <param name="contract">The contract type</param>
        /// <param name="endDateTime">The end date of the request</param>
        /// <param name="duration">The duration of the request</param>
        /// <param name="barSizeSetting">The bar size to request</param>
        /// <param name="whatToShow">The historical data request type</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<HistoricalDataEventArgs>> GetHistoricalDataAsync(Contract contract, DateTime endDateTime, string duration, string barSizeSetting, string whatToShow);


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

        void RequestMarketDataType(int marketDataType);
    }
}
