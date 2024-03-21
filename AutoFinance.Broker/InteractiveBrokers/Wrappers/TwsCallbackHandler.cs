// Licensed under the Apache License, Version 2.0.

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
        /// The event that is fired when continuous HistoricalDataUpdateEvent is called by TWS
        /// </summary>
        public event EventHandler<HistoricalDataEventArgs> HistoricalDataUpdateEvent;

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
        /// The event that is fired on a tick price event
        /// </summary>
        public event EventHandler<TickPriceEventArgs> TickPriceEvent;

        /// <summary>
        /// The event that is fired on a tick EFP event
        /// </summary>
        public event EventHandler<TickEFPEventArgs> TickEFPEvent;

        /// <summary>
        /// The event that is fired on a tick size event
        /// </summary>
        public event EventHandler<TickSizeEventArgs> TickSizeEvent;

        /// <summary>
        /// The event that is fired on a tick string event
        /// </summary>
        public event EventHandler<TickStringEventArgs> TickStringEvent;

        /// <summary>
        /// The event that is fired on a tick generic event
        /// </summary>
        public event EventHandler<TickGenericEventArgs> TickGenericEvent;

        /// <summary>
        /// The event that is fired on a tick option computation event
        /// </summary>
        public event EventHandler<TickOptionComputationEventArgs> TickOptionComputationEvent;

        /// <summary>
        /// The event that is fired on a tick snapshot end event
        /// </summary>
        public event EventHandler<TickSnapshotEndEventArgs> TickSnapshotEndEvent;

        /// <summary>
        /// The event that is fired on a tick req params event
        /// </summary>
        public event EventHandler<TickReqParamsEventArgs> TickReqParamsEvent;

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
        public event EventHandler<RequestIdEventArgs> SecurityDefinitionOptionParameterEndEvent;

        /// <summary>
        /// The event that is fired at the end of the account PnL request
        /// </summary>
        public event EventHandler<PnLEventArgs> PnLEvent;

        /// <summary>
        /// The event that is fired at the end of the single position PnL request
        /// </summary>
        public event EventHandler<PnLSingleEventArgs> PnLSingleEvent;

        /// <summary>
        /// The event that is fired when the account summary is received.
        /// </summary>
        public event EventHandler<AccountSummaryEventArgs> AccountSummaryEvent;

        /// <summary>
        /// The event that is fired when the account summary request is completed.
        /// </summary>
        public event EventHandler<RequestEndEventArgs> AccountSummaryEndEvent;

        /// <summary>
        /// The event that is fired when the account updates are received.
        /// </summary>
        public event EventHandler<AccountUpdateMultiEventArgs> AccountUpdateMultiEvent;

        /// <summary>
        /// The event that is fired when the account updates are completely received.
        /// </summary>
        public event EventHandler<RequestEndEventArgs> AccountUpdateMultiEndEvent;

        /// <summary>
        /// The event that is fired when the minimum price increments are returned for a market rule.
        /// </summary>
        public event EventHandler<MarketRuleEventArgs> MarketRuleEvent;

        /// <summary>
        /// The event that is fired when the commission report of an execution is received.
        /// </summary>
        public event EventHandler<CommissionReportEventArgs> CommissionReportEvent;

        /// <summary>
        /// The event that is fired when a completed order is received.
        /// </summary>
        public event EventHandler<CompletedOrderEventArgs> CompletedOrderEvent;

        /// <summary>
        /// The event that is fired when all completed orders are fetched.
        /// </summary>
        public event EventHandler CompletedOrdersEndEvent;

        /// <summary>
        /// The event that is fired when a bond contract data is  received.
        /// </summary>
        public event EventHandler<BondContractDetailsEventArgs> BondContractDetailsEvent;

        /// <summary>
        /// The event that is fired when histogram data is received.
        /// </summary>
        public event EventHandler<HistogramDataEventArgs> HistogramDataEvent;

        /// <summary>
        /// When historical ticks event are received.
        /// </summary>
        public event EventHandler<HistoricalTicksEventArgs> HistoricalTicksEvent;

        /// <summary>
        /// The event that is fired when historical ticks bid ask data is received.
        /// </summary>
        public event EventHandler<HistoricalTicksBidAskEventArgs> HistoricalTicksBidAskEvent;

        /// <summary>
        /// The event that is fired when historical ticks last are received.
        /// </summary>
        public event EventHandler<HistoricalTicksLastEventArgs> HistoricalTicksLastEvent;

        /// <summary>
        /// The event that is fired when market data types are received.
        /// </summary>
        public event EventHandler<MarketDataTypeEventArgs> MarketDataTypeEvent;

        /// <summary>
        /// The event that is fired when an order bind has been received.
        /// </summary>
        public event EventHandler<OrderBoundEventArgs> OrderBoundEvent;

        /// <summary>
        /// The event that is fired when a portfolio's open position is received.
        /// </summary>
        public event EventHandler<PositionMultiEventArgs> PositionMultiEvent;

        /// <summary>
        /// The event that is fired when all portfolio's open positions are received.
        /// </summary>
        public event EventHandler<RequestEndEventArgs> RequestPositionsMultiEndEvent;

        /// <summary>
        /// The event that is fired when a portfolio has been updated.
        /// </summary>
        public event EventHandler<UpdatePortfolioEventArgs> UpdatePortfolioEvent;

        /// <inheritdoc/>
        public void accountDownloadEnd(string account)
        {
            var eventArgs = new AccountDownloadEndEventArgs(account);
            this.AccountDownloadEndEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void accountSummary(int reqId, string account, string tag, string value, string currency)
        {
            var eventArgs = new AccountSummaryEventArgs(reqId, account, tag, value, currency);
            this.AccountSummaryEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void accountSummaryEnd(int reqId)
        {
            this.AccountSummaryEndEvent?.Invoke(this, new RequestEndEventArgs(reqId));
        }

        /// <inheritdoc/>
        public void accountUpdateMulti(int requestId, string account, string modelCode, string key, string value, string currency)
        {
            var eventArgs = new AccountUpdateMultiEventArgs(requestId, account, modelCode, key, value, currency);
            this.AccountUpdateMultiEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void accountUpdateMultiEnd(int requestId)
        {
            this.AccountUpdateMultiEndEvent?.Invoke(this, new RequestEndEventArgs(requestId));
        }

        /// <inheritdoc/>
        public void bondContractDetails(int reqId, ContractDetails contract)
        {
            var eventArgs = new BondContractDetailsEventArgs(reqId, contract);
            this.BondContractDetailsEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void commissionReport(CommissionReport commissionReport)
        {
            this.CommissionReportEvent?.Invoke(this, new CommissionReportEventArgs(commissionReport));
        }

        /// <inheritdoc/>
        public void completedOrder(Contract contract, Order order, OrderState orderState)
        {
            var eventArgs = new CompletedOrderEventArgs(contract, order, orderState);
            this.CompletedOrderEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void completedOrdersEnd()
        {
            this.CompletedOrdersEndEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void connectAck()
        {
            this.ConnectionAcknowledgementEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void connectionClosed()
        {
            this.ConnectionClosedEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void contractDetails(int reqId, ContractDetails contractDetails)
        {
            // Raise an event here which can be listened throughout the application
            var eventArgs = new ContractDetailsEventArgs(reqId, contractDetails);
            this.ContractDetailsEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void contractDetailsEnd(int reqId)
        {
            // Raise an event which can be listened throughout the application
            var eventArgs = new ContractDetailsEndEventArgs(reqId);
            this.ContractDetailsEndEvent?.Invoke(this, eventArgs);
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
            var eventArgs = new ErrorEventArgs(-1, -1, str, "");
            this.ErrorEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void error(int id, int errorCode, string errorMsg, string advancedOrderRejectJson)
        {
            var eventArgs = new ErrorEventArgs(id, errorCode, errorMsg, advancedOrderRejectJson);
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
            var eventArgs = new HistogramDataEventArgs(reqId, data);
            this.HistogramDataEvent?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// A callback for historical data, fixing a breaking change by the IB API to keep this signature
        /// </summary>
        /// <param name="reqId">The req id.</param>
        /// <param name="date">The date.</param>
        /// <param name="open">The open.</param>
        /// <param name="high">The high.</param>
        /// <param name="low">The low.</param>
        /// <param name="close">The close.</param>
        /// <param name="volume">The volume.</param>
        /// <param name="count">The count.</param>
        /// <param name="WAP">The WAP.</param>
        /// <param name="hasGaps">Whether the data has gaps.</param>
        public void historicalData(int reqId, string date, double open, double high, double low, double close, decimal volume, int count, decimal WAP, bool hasGaps)
        {
            // Raise an event which can be listened throughout the application
            var eventArgs = new HistoricalDataEventArgs(reqId, date, open, high, low, close, volume, count, WAP, hasGaps);
            this.HistoricalDataEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void historicalData(int reqId, Bar bar)
        {
            this.historicalData(reqId, bar.Time, bar.Open, bar.High, bar.Low, bar.Close, bar.Volume, bar.Count, bar.WAP, false);
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
            // Raise an event which can be listened throughout the application
            var eventArgs = new HistoricalDataEventArgs(reqId, bar.Time, bar.Open, bar.High, bar.Low, bar.Close, bar.Volume, bar.Count, bar.WAP, false);
            this.HistoricalDataUpdateEvent?.Invoke(this, eventArgs);
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

        public void historicalSchedule(int reqId, string startDateTime, string endDateTime, string timeZone, HistoricalSession[] sessions)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void historicalTicks(int reqId, HistoricalTick[] ticks, bool done)
        {
            var eventArgs = new HistoricalTicksEventArgs(reqId, ticks, done);
            this.HistoricalTicksEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void historicalTicksBidAsk(int reqId, HistoricalTickBidAsk[] ticks, bool done)
        {
            var eventArgs = new HistoricalTicksBidAskEventArgs(reqId, ticks, done);
            this.HistoricalTicksBidAskEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void historicalTicksLast(int reqId, HistoricalTickLast[] ticks, bool done)
        {
            var eventArgs = new HistoricalTicksLastEventArgs(reqId, ticks, done);
            this.HistoricalTicksLastEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void managedAccounts(string accountsList)
        {
        }

        /// <inheritdoc/>
        public void marketDataType(int reqId, int marketDataType)
        {
            var eventArgs = new MarketDataTypeEventArgs(reqId, marketDataType);
            this.MarketDataTypeEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void marketRule(int marketRuleId, PriceIncrement[] priceIncrements)
        {
            var eventArgs = new MarketRuleEventArgs(marketRuleId, priceIncrements);
            this.MarketRuleEvent?.Invoke(this, eventArgs);
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
            var eventArgs = new OrderBoundEventArgs(orderId, apiClientId, apiOrderId);
            this.OrderBoundEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void orderStatus(int orderId, string status, decimal filled, decimal remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
        {
            var eventArgs = new OrderStatusEventArgs(orderId, status, filled, remaining, avgFillPrice, permId, parentId, lastFillPrice, clientId, whyHeld);
            this.OrderStatusEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void pnl(int reqId, double dailyPnL, double unrealizedPnL, double realizedPnL)
        {
            var eventArgs = new PnLEventArgs(reqId, dailyPnL, unrealizedPnL, realizedPnL);
            this.PnLEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void pnlSingle(int reqId, decimal pos, double dailyPnL, double unrealizedPnL, double realizedPnL, double value)
        {
            var eventArgs = new PnLSingleEventArgs(reqId, pos, dailyPnL, unrealizedPnL, realizedPnL, value);
            this.PnLSingleEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void position(string account, Contract contract, decimal pos, double avgCost)
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
        public void positionMulti(int requestId, string account, string modelCode, Contract contract, decimal pos, double avgCost)
        {
            var eventArgs = new PositionMultiEventArgs(requestId, account, modelCode, contract, pos, avgCost);
            this.PositionMultiEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void positionMultiEnd(int requestId)
        {
            this.RequestPositionsMultiEndEvent?.Invoke(this, new RequestEndEventArgs(requestId));
        }

        /// <inheritdoc/>
        public void realtimeBar(int reqId, long time, double open, double high, double low, double close, decimal volume, decimal WAP, int count)
        {
            var eventArgs = new RealtimeBarEventArgs(reqId, time, open, high, low, close, volume, WAP, count);
            this.RealtimeBarEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void receiveFA(int faDataType, string faXmlData)
        {
            throw new NotImplementedException();
        }

        public void replaceFAEnd(int reqId, string text)
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
            var eventArgs = new RequestIdEventArgs(reqId);
            this.SecurityDefinitionOptionParameterEndEvent?.Invoke(this, eventArgs);
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
        public void tickByTickAllLast(int reqId, int tickType, long time, double price, decimal size, TickAttribLast tickAttriblast, string exchange, string specialConditions)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void tickByTickBidAsk(int reqId, long time, double bidPrice, double askPrice, decimal bidSize, decimal askSize, TickAttribBidAsk tickAttribBidAsk)
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
            var eventArgs = new TickEFPEventArgs(
                tickerId,
                tickType,
                basisPoints,
                formattedBasisPoints,
                impliedFuture,
                holdDays,
                futureLastTradeDate,
                dividendImpact,
                dividendsToLastTradeDate);
            this.TickEFPEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void tickGeneric(int tickerId, int field, double value)
        {
            var eventArgs = new TickGenericEventArgs(tickerId, field, value);
            this.TickGenericEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void tickNews(int tickerId, long timeStamp, string providerCode, string articleId, string headline, string extraData)
        {
            var eventArgs = new TickNewsEventArgs(tickerId, timeStamp, providerCode, articleId, headline, extraData);
            this.TickNewsEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void tickOptionComputation(int tickerId, int field, int tickAttrib, double impliedVolatility, double delta, double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
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
            var eventArgs = new TickPriceEventArgs(tickerId, field, price, attribs);
            this.TickPriceEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void tickReqParams(int tickerId, double minTick, string bboExchange, int snapshotPermissions)
        {
            var eventArgs = new TickReqParamsEventArgs(tickerId, minTick, bboExchange, snapshotPermissions);
            this.TickReqParamsEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void tickSize(int tickerId, int field, decimal size)
        {
            var eventArgs = new TickSizeEventArgs(tickerId, field, size);
            this.TickSizeEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void tickSnapshotEnd(int tickerId)
        {
            var eventArgs = new TickSnapshotEndEventArgs(tickerId);
            this.TickSnapshotEndEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void tickString(int tickerId, int field, string value)
        {
            var eventArgs = new TickStringEventArgs(tickerId, field, value);
            this.TickStringEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void updateAccountTime(string timestamp)
        {
        }

        /// <inheritdoc/>
        public void updateAccountValue(string key, string value, string currency, string accountName)
        {
            var eventArgs = new UpdateAccountValueEventArgs(key, value, currency, accountName);
            this.UpdateAccountValueEvent?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public void updateMktDepth(int tickerId, int position, int operation, int side, double price, decimal size)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, decimal side, double price, int size)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side, double price, decimal size, bool isSmartDepth)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void updateNewsBulletin(int msgId, int msgType, string message, string origExchange)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void updatePortfolio(Contract contract, decimal position, double marketPrice, double marketValue, double averageCost, double unrealisedPNL, double realisedPNL, string accountName)
        {
            var eventArgs = new UpdatePortfolioEventArgs(
                contract,
                position,
                marketPrice,
                marketValue,
                averageCost,
                unrealisedPNL,
                realisedPNL,
                accountName);

            this.UpdatePortfolioEvent?.Invoke(this, eventArgs);
        }

        public void userInfo(int reqId, string whiteBrandingId)
        {
            throw new NotImplementedException();
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

        public void wshEventData(int reqId, string dataJson)
        {
            throw new NotImplementedException();
        }

        public void wshMetaData(int reqId, string dataJson)
        {
            throw new NotImplementedException();
        }
    }
}
