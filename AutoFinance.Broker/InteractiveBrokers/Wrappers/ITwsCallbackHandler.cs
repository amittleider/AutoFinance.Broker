// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Wrappers
{
    using System;
    using System.Collections.Generic;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using IBApi;

    /// <summary>
    /// This interface is a combination of the EWrapper plus events.
    /// The interface allows easy unit testing.
    /// </summary>
    public interface ITwsCallbackHandler
    {
        /// <summary>
        /// The event that is fired when AccountDownloadEnd is called by TWS
        /// </summary>
        event EventHandler<AccountDownloadEndEventArgs> AccountDownloadEndEvent;

        /// <summary>
        /// The event that is fired when the ContractDetailsEnd is called by TWS
        /// </summary>
        event EventHandler<ContractDetailsEndEventArgs> ContractDetailsEndEvent;

        /// <summary>
        /// The event that is fired when ContractDetails is called by TWS
        /// </summary>
        event EventHandler<ContractDetailsEventArgs> ContractDetailsEvent;

        /// <summary>
        /// The event that is fired when the connection is acknowledged by TWS
        /// </summary>
        event EventHandler ConnectionAcknowledgementEvent;

        /// <summary>
        /// The event that is fired when the connection is acknowledged by TWS
        /// </summary>
        event EventHandler ConnectionClosedEvent;

        /// <summary>
        /// The event that is fired when Error is called by TWS
        /// </summary>
        event EventHandler<ErrorEventArgs> ErrorEvent;

        /// <summary>
        /// The event that is fired when NextValidId is called by TWS
        /// </summary>
        event EventHandler<NextValidIdEventArgs> NextValidIdEvent;

        /// <summary>
        /// The event that is fired when OpenOrder is called by TWS
        /// </summary>
        event EventHandler<OpenOrderEventArgs> OpenOrderEvent;

        /// <summary>
        /// The event that is fired when OpenOrderEnd is called by TWS
        /// </summary>
        event EventHandler<OpenOrderEndEventArgs> OpenOrderEndEvent;

        /// <summary>
        /// The event that is fired when OrderStatus is called by TWS
        /// </summary>
        event EventHandler<OrderStatusEventArgs> OrderStatusEvent;

        /// <summary>
        /// The event that is fired when HistoricalData is called by TWS
        /// </summary>
        event EventHandler<HistoricalDataEventArgs> HistoricalDataEvent;

        /// <summary>
        /// The event that is fired when continuous HistoricalDataUpdateEvent is called by TWS
        /// </summary>
        event EventHandler<HistoricalDataEventArgs> HistoricalDataUpdateEvent;

        /// <summary>
        /// The event that is fired when HistoricalDataEnd is called by TWS
        /// </summary>
        event EventHandler<HistoricalDataEndEventArgs> HistoricalDataEndEvent;

        /// <summary>
        /// The event that is fired when RealtimeBar is called by TWS
        /// </summary>
        event EventHandler<RealtimeBarEventArgs> RealtimeBarEvent;

        /// <summary>
        /// The event that is fired when AccountUpdates is called by TWS
        /// </summary>
        event EventHandler<UpdateAccountValueEventArgs> UpdateAccountValueEvent;

        /// <summary>
        /// The event that is fired when Position is called by TWS
        /// </summary>
        event EventHandler<PositionStatusEventArgs> PositionStatusEvent;

        /// <summary>
        /// The event that is fired when PositionEnd is called by TWS
        /// </summary>
        event EventHandler<RequestPositionsEndEventArgs> RequestPositionsEndEvent;

        /// <summary>
        /// The event that is fired when Executions
        /// </summary>
        event EventHandler<ExecutionDetailsEventArgs> ExecutionDetailsEvent;

        /// <summary>
        /// The event that is fired when Executions end
        /// </summary>
        event EventHandler<ExecutionDetailsEndEventArgs> ExecutionDetailsEndEvent;

        /// <summary>
        /// The event that is fired on a tick news event
        /// </summary>
        event EventHandler<TickNewsEventArgs> TickNewsEvent;

        /// <summary>
        /// The event that is fired on news provider events
        /// </summary>
        event EventHandler<NewsProviderEventArgs> NewsProviderEvent;

        /// <summary>
        /// The event that is fired when option security definitions are requested
        /// </summary>
        event EventHandler<SecurityDefinitionOptionParameterEventArgs> SecurityDefinitionOptionParameterEvent;

        /// <summary>
        /// The event that is fired at the end of the option security definition request
        /// </summary>
        event EventHandler<RequestIdEventArgs> SecurityDefinitionOptionParameterEndEvent;

        /// <summary>
        /// The event that is fired at the end of the account PnL request
        /// </summary>
        event EventHandler<PnLEventArgs> PnLEvent;

        /// <summary>
        /// The event that is fired at the end of the single position PnL request
        /// </summary>
        event EventHandler<PnLSingleEventArgs> PnLSingleEvent;

        /// <summary>
        /// The event that is fired when the account summary is received.
        /// </summary>
        event EventHandler<AccountSummaryEventArgs> AccountSummaryEvent;

        /// <summary>
        /// The event that is fired when the account summary request is completed.
        /// </summary>
        event EventHandler<RequestEndEventArgs> AccountSummaryEndEvent;

        /// <summary>
        /// The event that is fired when the account updates are received.
        /// </summary>
        event EventHandler<AccountUpdateMultiEventArgs> AccountUpdateMultiEvent;

        /// <summary>
        /// The event that is fired when the account updates are completely received.
        /// </summary>
        event EventHandler<RequestEndEventArgs> AccountUpdateMultiEndEvent;

        /// <summary>
        /// The event that is fired when the minimum price increments are returned for a market rule.
        /// </summary>
        event EventHandler<MarketRuleEventArgs> MarketRuleEvent;

        /// <summary>
        /// The event that is fired when the commission report of an execution is received.
        /// </summary>
        event EventHandler<CommissionReportEventArgs> CommissionReportEvent;

        /// <summary>
        /// The event that is fired when a completed order is received.
        /// </summary>
        event EventHandler<CompletedOrderEventArgs> CompletedOrderEvent;

        /// <summary>
        /// The event that is fired when all completed orders are fetched.
        /// </summary>
        event EventHandler CompletedOrdersEndEvent;

        /// <summary>
        /// The event that is fired when a bond contract data is  received.
        /// </summary>
        event EventHandler<BondContractDetailsEventArgs> BondContractDetailsEvent;

        /// <summary>
        /// The event that is fired when histogram data is received.
        /// </summary>
        event EventHandler<HistogramDataEventArgs> HistogramDataEvent;

        /// <summary>
        /// When historical ticks event are received.
        /// </summary>
        event EventHandler<HistoricalTicksEventArgs> HistoricalTicksEvent;

        /// <summary>
        /// The event that is fired when historical ticks bid ask data is received.
        /// </summary>
        event EventHandler<HistoricalTicksBidAskEventArgs> HistoricalTicksBidAskEvent;

        /// <summary>
        /// The event that is fired when historical ticks last are received.
        /// </summary>
        event EventHandler<HistoricalTicksLastEventArgs> HistoricalTicksLastEvent;

        /// <summary>
        /// The event that is fired when market data types are received.
        /// </summary>
        event EventHandler<MarketDataTypeEventArgs> MarketDataTypeEvent;

        /// <summary>
        /// The event that is fired when an order bind has been received.
        /// </summary>
        event EventHandler<OrderBoundEventArgs> OrderBoundEvent;

        /// <summary>
        /// The event that is fired when a portfolio's open position is received.
        /// </summary>
        event EventHandler<PositionMultiEventArgs> PositionMultiEvent;

        /// <summary>
        /// The event that is fired when all portfolio's open positions are received.
        /// </summary>
        event EventHandler<RequestEndEventArgs> RequestPositionsMultiEndEvent;

        /// <summary>
        /// The event that is fired when a portfolio has been updated.
        /// </summary>
        event EventHandler<UpdatePortfolioEventArgs> UpdatePortfolioEvent;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements must be documented
#pragma warning disable SA1300 // Element must begin with upper-case letter

        /// <summary>
        /// This is called at the end of an AccountDetails request
        /// </summary>
        /// <param name="account">The account Id</param>
        void accountDownloadEnd(string account);

        /// <summary>
        /// Account Summary
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="account">The account</param>
        /// <param name="tag">The tag</param>
        /// <param name="value">The value</param>
        /// <param name="currency">The currency</param>
        void accountSummary(int reqId, string account, string tag, string value, string currency);

        /// <summary>
        /// Account summary end callback
        /// </summary>
        /// <param name="reqId">The request Id</param>
        void accountSummaryEnd(int reqId);

        /// <summary>
        /// The account update multi callback
        /// </summary>
        /// <param name="requestId">The request Id</param>
        /// <param name="account">The Account</param>
        /// <param name="modelCode">The model code</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <param name="currency">The currency</param>
        void accountUpdateMulti(int requestId, string account, string modelCode, string key, string value, string currency);

        /// <summary>
        /// The account update multi end callback
        /// </summary>
        /// <param name="requestId">The request Id</param>
        void accountUpdateMultiEnd(int requestId);

        /// <summary>
        /// The bond contract details callback
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="contract">The contract</param>
        void bondContractDetails(int reqId, ContractDetails contract);

        /// <summary>
        /// The commission report callback
        /// </summary>
        /// <param name="commissionReport">The commission report</param>
        void commissionReport(CommissionReport commissionReport);

        /// <summary>
        /// The connection acknowledgement
        /// </summary>
        void connectAck();

        /// <summary>
        /// The connection closed event
        /// </summary>
        void connectionClosed();

        /// <summary>
        /// The contract details callback
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="contractDetails">The contract details</param>
        void contractDetails(int reqId, ContractDetails contractDetails);

        /// <summary>
        /// The contract details end callback
        /// </summary>
        /// <param name="reqId">The request Id</param>
        void contractDetailsEnd(int reqId);

        /// <summary>
        /// The current time callback
        /// </summary>
        /// <param name="time">The time</param>
        void currentTime(long time);

        /// <summary>
        /// The delta neutral validation callback
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="deltaNeutralContract">The deltaNeutralContract</param>
        void deltaNeutralValidation(int reqId, DeltaNeutralContract deltaNeutralContract);

        /// <summary>
        /// The display group list callback
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="groups">The groups</param>
        void displayGroupList(int reqId, string groups);

        /// <summary>
        /// The display group updated callback
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="contractInfo">The contract info</param>
        void displayGroupUpdated(int reqId, string contractInfo);

        /// <summary>
        /// Error callback from TWS
        /// </summary>
        /// <param name="e">The exception</param>
        void error(Exception e);

        /// <summary>
        /// Error callback from TWS
        /// </summary>
        /// <param name="id">The request Id (?)</param>
        /// <param name="errorCode">The error code</param>
        /// <param name="errorMsg">The error message</param>
        void error(int id, int errorCode, string errorMsg);

        /// <summary>
        /// The error callback from TWS
        /// </summary>
        /// <param name="str">The error message (?)</param>
        void error(string str);

        /// <summary>
        /// The execution details from TWS
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="contract">The contract</param>
        /// <param name="execution">The execution</param>
        void execDetails(int reqId, Contract contract, Execution execution);

        /// <summary>
        /// The exec details end callback from TWS
        /// </summary>
        /// <param name="reqId">The request Id sent to a call to ExecDetails</param>
        void execDetailsEnd(int reqId);

        /// <summary>
        /// The fundamental data callback from TWS
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="data">The data (itself)(?)</param>
        void fundamentalData(int reqId, string data);

        /// <summary>
        /// The historical data callback from TWS
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="date">The date</param>
        /// <param name="open">The open</param>
        /// <param name="high">The high</param>
        /// <param name="low">The low</param>
        /// <param name="close">The close</param>
        /// <param name="volume">The volume</param>
        /// <param name="count">The count</param>
        /// <param name="WAP">The weighted average price</param>
        /// <param name="hasGaps">If the data has gaps</param>
        void historicalData(int reqId, string date, double open, double high, double low, double close, int volume, int count, double WAP, bool hasGaps);

        void historicalDataEnd(int reqId, string start, string end);

        /// <summary>
        /// The account PnL request data callback from TWS
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="dailyPnL">The daily PnL</param>
        /// <param name="unrealizedPnL">The unrealized PnL</param>
        /// <param name="realizedPnL">The realized PnL</param>
        void pnl(int reqId, double dailyPnL, double unrealizedPnL, double realizedPnL);

        /// <summary>
        /// The single position PnL data callback from TWS
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <param name="pos">The position</param>
        /// <param name="dailyPnL">The daily PnL</param>
        /// <param name="unrealizedPnL">The unrealized PnL</param>
        /// <param name="realizedPnL">The realized PnL</param>
        /// <param name="value">The position value</param>
        void pnlSingle(int reqId, int pos, double dailyPnL, double unrealizedPnL, double realizedPnL, double value);

        void managedAccounts(string accountsList);

        void marketDataType(int reqId, int marketDataType);

        void nextValidId(int orderId);

        void openOrder(int orderId, Contract contract, Order order, OrderState orderState);

        void openOrderEnd();

        void orderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice);

        void position(string account, Contract contract, double pos, double avgCost);

        void positionEnd();

        void positionMulti(int requestId, string account, string modelCode, Contract contract, double pos, double avgCost);

        void positionMultiEnd(int requestId);

        void realtimeBar(int reqId, long time, double open, double high, double low, double close, long volume, double WAP, int count);

        void receiveFA(int faDataType, string faXmlData);

        void scannerData(int reqId, int rank, ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr);

        void scannerDataEnd(int reqId);

        void scannerParameters(string xml);

        void securityDefinitionOptionParameter(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes);

        void securityDefinitionOptionParameterEnd(int reqId);

        void softDollarTiers(int reqId, SoftDollarTier[] tiers);

        void tickEFP(int tickerId, int tickType, double basisPoints, string formattedBasisPoints, double impliedFuture, int holdDays, string futureLastTradeDate, double dividendImpact, double dividendsToLastTradeDate);

        void tickGeneric(int tickerId, int field, double value);

        void tickOptionComputation(int tickerId, int field, double impliedVolatility, double delta, double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice);

        void tickPrice(int tickerId, int field, double price, int canAutoExecute);

        void tickSize(int tickerId, int field, int size);

        void tickSnapshotEnd(int tickerId);

        void tickString(int tickerId, int field, string value);

        void updateAccountTime(string timestamp);

        void updateAccountValue(string key, string value, string currency, string accountName);

        void updateMktDepth(int tickerId, int position, int operation, int side, double price, int size);

        void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side, double price, int size);

        void updateNewsBulletin(int msgId, int msgType, string message, string origExchange);

        void updatePortfolio(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealisedPNL, double realisedPNL, string accountName);

        void verifyAndAuthCompleted(bool isSuccessful, string errorText);

        void verifyAndAuthMessageAPI(string apiData, string xyzChallenge);

        void verifyCompleted(bool isSuccessful, string errorText);

        void verifyMessageAPI(string apiData);
#pragma warning restore SA1300 // Element must begin with upper-case letter
#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
