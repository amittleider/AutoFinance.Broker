// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Wrappers
{
    using System.Collections.Generic;
    using IBApi;

    /// <summary>
    /// This interface is a wrapper for the TWS EClient
    /// It is used to make the EClient unit-testable
    /// All calls here have return values specified in the TWSCallbackHandler
    /// </summary>
    public interface ITwsClientSocket
    {
        /// <summary>
        /// Gets the TWS socket connection
        /// </summary>
        EClientSocket EClientSocket
        {
            get;
        }

        /// <summary>
        /// Connects the client to the socket
        /// </summary>
        /// <param name="host">The hostname</param>
        /// <param name="port">The port</param>
        /// <param name="clientId">The client Id</param>
        void Connect(string host, int port, int clientId);

        /// <summary>
        /// Disconnects the client from the socket
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Sends an order to TWS through the socket
        /// </summary>
        /// <param name="id">The order Id</param>
        /// <param name="contract">The contract to trade</param>
        /// <param name="order">The order to place</param>
        void PlaceOrder(int id, Contract contract, Order order);

        /// <summary>
        /// Cancel an order from TWS
        /// </summary>
        /// <param name="orderId">The order Id to cancel</param>
        void CancelOrder(int orderId);

        /// <summary>
        /// Requests details of a contract from TWS
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="contract">The contract</param>
        void ReqContractDetails(int reqId, Contract contract);

        /// <summary>
        /// Request account details from TWS
        /// </summary>
        /// <param name="subscribe">Subscribe the the stream</param>
        /// <param name="accountCode">The account of the details requested</param>
        void ReqAccountDetails(bool subscribe, string accountCode);

        /// <summary>
        /// Request open orders
        /// </summary>
        void RequestAllOpenOrders();

        /// <summary>
        /// Request historical data from TWS
        /// </summary>
        /// <param name="requestId">The request Id. IB messed up the naming convention and sometimes calls this "tickerId".</param>
        /// <param name="contract">The contract</param>
        /// <param name="endDateTime">The end time</param>
        /// <param name="durationString">The duration string</param>
        /// <param name="barSizeSetting">The bar size setting</param>
        /// <param name="whatToShow">The things to show. Available options are 'TRADES', 'BID', 'ASK', and 'MIDPOINT'.</param>
        /// <param name="useRTH">Whether to use regular trading hours</param>
        /// <param name="formatDate">Whether to format the date</param>
        /// <param name="chartOptions">The chart options</param>
        /// <param name="keepUpToDate">Keep up to date or not</param>
        void ReqHistoricalData(int requestId, Contract contract, string endDateTime, string durationString, string barSizeSetting, string whatToShow, int useRTH, int formatDate, List<TagValue> chartOptions, bool keepUpToDate = false);

        /// <summary>
        /// Request realtime data from TWS
        /// </summary>
        /// <param name="requestId">The request Id. IB messed up the naming convention and sometimes calls this "tickerId".</param>
        /// <param name="contract">The contract</param>
        /// <param name="barSize">The bar size</param>
        /// <param name="whatToShow">The things to show (?)</param>
        /// <param name="useRTH">Whether to use regular trading hours</param>
        /// <param name="realtimeBarOptions">The realtime bar options</param>
        void ReqRealtimeBars(int requestId, Contract contract, int barSize, string whatToShow, bool useRTH, List<TagValue> realtimeBarOptions);

        /// <summary>
        /// Sends a message to TWS telling it to cancel a real-time bar subscription
        /// </summary>
        /// <param name="requestId">The request Id of the subscription for which to cancel</param>
        void CancelRealtimeBars(int requestId);

        /// <summary>
        /// Sends a message to TWS telling it to send position information through the socket.
        /// </summary>
        void RequestPositions();

        /// <summary>
        /// Sends a message to TWS telling it to send execution information through the socket.
        /// </summary>
        /// <param name="requestId">The request Id</param>
        void RequestExecutions(int requestId);

        /// <summary>
        /// Request market data (for news as well)
        /// </summary>
        /// <param name="requestId">The request</param>
        /// <param name="contract">The contract</param>
        /// <param name="genericTickList">The generic tick list</param>
        /// <param name="snapshot">The snapshot</param>
        /// <param name="regulatorySnaphsot">The regulatory snapshot</param>
        /// <param name="marketDataOptions">The market data options</param>
        void RequestMarketData(int requestId, Contract contract, string genericTickList, bool snapshot, bool regulatorySnaphsot, List<TagValue> marketDataOptions);

        /// <summary>
        /// Request option chain details
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="underlyingSymbol">The underlying symbol</param>
        /// <param name="futFopExchange">The exchange</param>
        /// <param name="underlyingSecType">The underlying security type</param>
        /// <param name="underlyingConId">The underlying contract id</param>
        void RequestSecurityDefinitionOptionParameters(int reqId, string underlyingSymbol, string futFopExchange, string underlyingSecType, int underlyingConId);
    }
}
