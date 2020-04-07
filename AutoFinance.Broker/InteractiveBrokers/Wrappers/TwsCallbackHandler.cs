﻿// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Wrappers
{
    using System;
    using System.Collections.Generic;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using IBApi;

    /// <summary>
    /// The main wrapper for the TWS API.
    /// </summary>
    public class TwsCallbackHandler : EWrapper, ITwsCallbackHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwsCallbackHandler"/> class.
        /// </summary>
        public TwsCallbackHandler()
        {
        }

        /// <summary>
        /// The event that is fired when AccountDownloadEnd is called by TWS
        /// </summary>
        public event EventHandler<AccountDownloadEndEventArgs> AccountDownloadEndEvent;

        /// <summary>
        /// The event that is fired when the ContractDetailsEnd is called by TWS
        /// </summary>
        public event EventHandler<ContractDetailsEndEventArgs> ContractDetailsEndEvent;

        /// <summary>
        /// The event that is fired when ContractDetails is called by TWS
        /// </summary>
        public event EventHandler<ContractDetailsEventArgs> ContractDetailsEvent;

        /// <summary>
        /// The event that is fired when the connection is acknowledged by TWS
        /// </summary>
        public event EventHandler ConnectionAcknowledgementEvent;

        /// <summary>
        /// The event that is fired when the connection is acknowledged by TWS
        /// </summary>
        public event EventHandler ConnectionClosedEvent;

        /// <summary>
        /// The event that is fired when Error is called by TWS
        /// </summary>
        public event EventHandler<ErrorEventArgs> ErrorEvent;

        /// <summary>
        /// The event that is fired when NextValidId is called by TWS
        /// </summary>
        public event EventHandler<NextValidIdEventArgs> NextValidIdEvent;

        /// <summary>
        /// The event that is fired when OpenOrder is called by TWS
        /// </summary>
        public event EventHandler<OpenOrderEventArgs> OpenOrderEvent;

        /// <summary>
        /// The event that is fired when OpenOrderEnd is called by TWS
        /// </summary>
        public event EventHandler<OpenOrderEndEventArgs> OpenOrderEndEvent;

        /// <summary>
        /// The event that is fired when OrderStatus is called by TWS
        /// </summary>
        public event EventHandler<OrderStatusEventArgs> OrderStatusEvent;

        /// <summary>
        /// The event that is fired when HistoricalData is called by TWS
        /// </summary>
        public event EventHandler<HistoricalDataEventArgs> HistoricalDataEvent;

        /// <summary>
        /// The event that is fired when HistoricalDataEnd is called by TWS
        /// </summary>
        public event EventHandler<HistoricalDataEndEventArgs> HistoricalDataEndEvent;

        /// <summary>
        /// The event that is fired when RealtimeBar is called by TWS
        /// </summary>
        public event EventHandler<RealtimeBarEventArgs> RealtimeBarEvent;

        /// <summary>
        /// The event that is fired when AccountUpdates is called by TWS
        /// </summary>
        public event EventHandler<UpdateAccountValueEventArgs> UpdateAccountValueEvent;

        /// <summary>
        /// The event that is fired when Position is called by TWS
        /// </summary>
        public event EventHandler<PositionStatusEventArgs> PositionStatusEvent;

        /// <summary>
        /// The event that is fired when PositionEnd is called by TWS
        /// </summary>
        public event EventHandler<RequestPositionsEndEventArgs> RequestPositionsEndEvent;

        /// <summary>
        /// The event that is fired when Executions
        /// </summary>
        public event EventHandler<ExecutionDetailsEventArgs> ExecutionDetailsEvent;

        /// <summary>
        /// The event that is fired when Executions end
        /// </summary>
        public event EventHandler<ExecutionDetailsEndEventArgs> ExecutionDetailsEndEvent;

        /// <summary>
        /// The event that is fired on a tick news event
        /// </summary>
        public event EventHandler<TickNewsEventArgs> TickNewsEvent;

        /// <summary>
        /// The event that is fired on news provider events
        /// </summary>
        public event EventHandler<NewsProviderEventArgs> NewsProviderEvent;

        /// <summary>
        /// The event that is fired when option security definitions are requested
        /// </summary>
        public event EventHandler<SecurityDefinitionOptionParameterEventArgs> SecurityDefinitionOptionParameterEvent;

        /// <summary>
        /// The event that is fired at the end of the option security definition request
        /// </summary>
        public event EventHandler SecurityDefinitionOptionParameterEndEvent;

        /// <inheritdoc/>
        public void accountDownloadEnd(string account)
        {
            var eventArgs = new AccountDownloadEndEventArgs(account);
            this.AccountDownloadEndEvent.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void accountSummary(int reqId, string account, string tag, string value, string currency)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void accountSummaryEnd(int reqId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void accountUpdateMulti(int requestId, string account, string modelCode, string key, string value, string currency)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void accountUpdateMultiEnd(int requestId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void bondContractDetails(int reqId, ContractDetails contract)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void commissionReport(CommissionReport commissionReport)
        {
            // Unimplemented because we don't care
        }

        /// <inheritdoc/>
        public void completedOrder(Contract contract, Order order, OrderState orderState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void completedOrdersEnd()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void connectAck()
        {
            this.ConnectionAcknowledgementEvent.Invoke(this, null);
        }

        /// <inheritdoc/>
        public void connectionClosed()
        {
            this.ConnectionClosedEvent.Invoke(this, null);
        }

        /// <inheritdoc/>
        public void contractDetails(int reqId, ContractDetails contractDetails)
        {
            // Raise an event here which can be listened throughout the application
            var eventArgs = new ContractDetailsEventArgs(reqId, contractDetails);
            this.ContractDetailsEvent.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void contractDetailsEnd(int reqId)
        {
            // Raise an event which can be listened throughout the application
            var eventArgs = new ContractDetailsEndEventArgs(reqId);
            this.ContractDetailsEndEvent.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void currentTime(long time)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void deltaNeutralValidation(int reqId, DeltaNeutralContract deltaNeutralContract)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void displayGroupList(int reqId, string groups)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void displayGroupUpdated(int reqId, string contractInfo)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void error(Exception e)
        {
            throw e;
        }

        /// <inheritdoc/>
        public void error(string str)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void error(int id, int errorCode, string errorMsg)
        {
            var eventArgs = new ErrorEventArgs(id, errorCode, errorMsg);
            this.ErrorEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void execDetails(int reqId, Contract contract, Execution execution)
        {
            // Raise an event that can be listened throughout the application
            var eventArgs = new ExecutionDetailsEventArgs(reqId, contract, execution);
            this.ExecutionDetailsEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void execDetailsEnd(int reqId)
        {
            var eventArgs = new ExecutionDetailsEndEventArgs(reqId);
            this.ExecutionDetailsEndEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void familyCodes(FamilyCode[] familyCodes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void fundamentalData(int reqId, string data)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void headTimestamp(int reqId, string headTimestamp)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void histogramData(int reqId, HistogramEntry[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A callback for historical data, fixing a breaking change by the IB API to keep this signature
        /// </summary>
        /// <param name="reqId">The req id</param>
        /// <param name="date">The date</param>
        /// <param name="open">The open</param>
        /// <param name="high">The high</param>
        /// <param name="low">The low</param>
        /// <param name="close">The close</param>
        /// <param name="volume">The volume</param>
        /// <param name="count">The count</param>
        /// <param name="WAP">The WAP</param>
        /// <param name="hasGaps">Whether the data has gaps</param>
        public void historicalData(int reqId, string date, double open, double high, double low, double close, int volume, int count, double WAP, bool hasGaps)
        {
            // Raise an event which can be listened throughout the application
            var eventArgs = new HistoricalDataEventArgs(reqId, date, open, high, low, close, volume, count, WAP, hasGaps);
            this.HistoricalDataEvent.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void historicalData(int reqId, Bar bar)
        {
            this.historicalData(reqId, bar.Time, bar.Open, bar.High, bar.Low, bar.Close, (int)bar.Volume, bar.Count, bar.WAP, false);
        }

        /// <inheritdoc/>
        public void historicalDataEnd(int reqId, string start, string end)
        {
            // Raise an event which can be listened throughout the application
            var eventArgs = new HistoricalDataEndEventArgs(reqId, start, end);
            this.HistoricalDataEndEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void historicalDataUpdate(int reqId, Bar bar)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void historicalNews(int requestId, string time, string providerCode, string articleId, string headline)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void historicalNewsEnd(int requestId, bool hasMore)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void historicalTicks(int reqId, HistoricalTick[] ticks, bool done)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void historicalTicksBidAsk(int reqId, HistoricalTickBidAsk[] ticks, bool done)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void historicalTicksLast(int reqId, HistoricalTickLast[] ticks, bool done)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void managedAccounts(string accountsList)
        {
        }

        /// <inheritdoc/>
        public void marketDataType(int reqId, int marketDataType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void marketRule(int marketRuleId, PriceIncrement[] priceIncrements)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void mktDepthExchanges(DepthMktDataDescription[] depthMktDataDescriptions)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void newsArticle(int requestId, int articleType, string articleText)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void newsProviders(NewsProvider[] newsProviders)
        {
            var eventArgs = new NewsProviderEventArgs(newsProviders);
            this.NewsProviderEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void nextValidId(int orderId)
        {
            var eventArgs = new NextValidIdEventArgs(orderId);
            this.NextValidIdEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void openOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
            var eventArgs = new OpenOrderEventArgs(orderId, contract, order, orderState);
            this.OpenOrderEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void openOrderEnd()
        {
            var eventArgs = new OpenOrderEndEventArgs();
            this.OpenOrderEndEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void orderBound(long orderId, int apiClientId, int apiOrderId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void orderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
        {
            var eventArgs = new OrderStatusEventArgs(orderId, status, filled, remaining, avgFillPrice, permId, parentId, lastFillPrice, clientId, whyHeld);
            this.OrderStatusEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void pnl(int reqId, double dailyPnL, double unrealizedPnL, double realizedPnL)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void pnlSingle(int reqId, int pos, double dailyPnL, double unrealizedPnL, double realizedPnL, double value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void position(string account, Contract contract, double pos, double avgCost)
        {
            var eventArgs = new PositionStatusEventArgs(account, contract, pos, avgCost);
            this.PositionStatusEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void positionEnd()
        {
            var eventArgs = new RequestPositionsEndEventArgs();
            this.RequestPositionsEndEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void positionMulti(int requestId, string account, string modelCode, Contract contract, double pos, double avgCost)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void positionMultiEnd(int requestId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void realtimeBar(int reqId, long time, double open, double high, double low, double close, long volume, double WAP, int count)
        {
            var eventArgs = new RealtimeBarEventArgs(reqId, time, open, high, low, close, volume, WAP, count);
            this.RealtimeBarEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void receiveFA(int faDataType, string faXmlData)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void rerouteMktDataReq(int reqId, int conId, string exchange)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void rerouteMktDepthReq(int reqId, int conId, string exchange)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void scannerData(int reqId, int rank, ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void scannerDataEnd(int reqId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void scannerParameters(string xml)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void securityDefinitionOptionParameter(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes)
        {
            var eventArgs = new SecurityDefinitionOptionParameterEventArgs(reqId, exchange, underlyingConId, tradingClass, multiplier, expirations, strikes);
            this.SecurityDefinitionOptionParameterEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void securityDefinitionOptionParameterEnd(int reqId)
        {
            this.SecurityDefinitionOptionParameterEndEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void smartComponents(int reqId, Dictionary<int, KeyValuePair<string, char>> theMap)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void softDollarTiers(int reqId, SoftDollarTier[] tiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void symbolSamples(int reqId, ContractDescription[] contractDescriptions)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void tickByTickAllLast(int reqId, int tickType, long time, double price, int size, TickAttribLast tickAttriblast, string exchange, string specialConditions)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void tickByTickBidAsk(int reqId, long time, double bidPrice, double askPrice, int bidSize, int askSize, TickAttribBidAsk tickAttribBidAsk)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void tickByTickMidPoint(int reqId, long time, double midPoint)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void tickEFP(int tickerId, int tickType, double basisPoints, string formattedBasisPoints, double impliedFuture, int holdDays, string futureLastTradeDate, double dividendImpact, double dividendsToLastTradeDate)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void tickGeneric(int tickerId, int field, double value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void tickNews(int tickerId, long timeStamp, string providerCode, string articleId, string headline, string extraData)
        {
            var eventArgs = new TickNewsEventArgs(tickerId, timeStamp, providerCode, articleId, headline, extraData);
            this.TickNewsEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void tickOptionComputation(int tickerId, int field, double impliedVolatility, double delta, double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void tickPrice(int tickerId, int field, double price, int canAutoExecute)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void tickPrice(int tickerId, int field, double price, TickAttrib attribs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void tickReqParams(int tickerId, double minTick, string bboExchange, int snapshotPermissions)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void tickSize(int tickerId, int field, int size)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void tickSnapshotEnd(int tickerId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void tickString(int tickerId, int field, string value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void updateAccountTime(string timestamp)
        {
        }

        /// <inheritdoc/>
        public void updateAccountValue(string key, string value, string currency, string accountName)
        {
            var eventArgs = new UpdateAccountValueEventArgs(key, value, currency, accountName);
            this.UpdateAccountValueEvent.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void updateMktDepth(int tickerId, int position, int operation, int side, double price, int size)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side, double price, int size)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side, double price, int size, bool isSmartDepth)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void updateNewsBulletin(int msgId, int msgType, string message, string origExchange)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void updatePortfolio(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealisedPNL, double realisedPNL, string accountName)
        {
        }

        /// <inheritdoc/>
        public void verifyAndAuthCompleted(bool isSuccessful, string errorText)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void verifyAndAuthMessageAPI(string apiData, string xyzChallenge)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void verifyCompleted(bool isSuccessful, string errorText)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void verifyMessageAPI(string apiData)
        {
            throw new NotImplementedException();
        }
    }
}
