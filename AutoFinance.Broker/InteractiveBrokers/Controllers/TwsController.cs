// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using AutoFinance.Broker.InteractiveBrokers.Exceptions;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;
    using IBApi;

    /// <summary>
    /// A top level controller which interacts with the base controller to do useful things
    /// </summary>
    public class TwsController : ITwsControllerBase
    {
        private ITwsControllerBase twsControllerBase;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsController"/> class.
        /// </summary>
        /// <param name="twsControllerBase">The base controller</param>
        public TwsController(ITwsControllerBase twsControllerBase)
        {
            this.twsControllerBase = twsControllerBase;
        }

        /// <summary>
        /// Gets a value indicating whether is the client connected to tws
        /// </summary>
        public bool Connected => this.twsControllerBase.Connected;

        /// <summary>
        /// Place a bracket order on TWS.
        /// The entry is a REL order (Market Peg), exits are stop loss and limit orders at the specified prices.
        /// </summary>
        /// <param name="contract">The contract to trade</param>
        /// <param name="entryAction">The entry action -- buy, sell, sshort</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="takePrice">The take price</param>
        /// <param name="stopPrice">The stop price</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<bool> PlaceBracketOrder(Contract contract, string entryAction, double quantity, double takePrice, double stopPrice)
        {
            await this.EnsureConnectedAsync();

            // Generate the order IDs
            int entryOrderId = await this.GetNextValidIdAsync();
            var takeProfitOrderId = await this.GetNextValidIdAsync();
            var stopOrderId = await this.GetNextValidIdAsync();

            // Initialize the order
            Order entryOrder = new Order()
            {
                Action = entryAction,
                OrderType = TwsOrderType.PegToMarket,
                TotalQuantity = quantity,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = false,
            };

            Order takeProfit = new Order()
            {
                Action = TwsOrderActions.Reverse(entryAction),
                OrderType = TwsOrderType.Limit,
                TotalQuantity = quantity,
                LmtPrice = takePrice,
                ParentId = entryOrderId,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = false,
            };

            Order stopLoss = new Order()
            {
                Action = TwsOrderActions.Reverse(entryAction),
                OrderType = TwsOrderType.StopLoss,
                TotalQuantity = quantity,
                AuxPrice = stopPrice,
                ParentId = entryOrderId,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = true,
            };

            var entryOrderAckTask = this.PlaceOrderAsync(entryOrderId, contract, entryOrder);
            var takeProfitOrderAckTask = this.PlaceOrderAsync(takeProfitOrderId, contract, takeProfit);
            var stopOrderAckTask = this.PlaceOrderAsync(stopOrderId, contract, stopLoss);
            Task.WaitAll(entryOrderAckTask, takeProfitOrderAckTask, stopOrderAckTask);

            return entryOrderAckTask.Result && takeProfitOrderAckTask.Result && stopOrderAckTask.Result;
        }

        /// <summary>
        /// Places a bracket order with a limit order entry
        /// </summary>
        /// <param name="contract">The contract</param>
        /// <param name="entryAction">Buy/sell/ssell</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="entryOrderPrice">The limit order entry price</param>
        /// <param name="takePrice">The take profit price</param>
        /// <param name="stopPrice">The stop loss price</param>
        /// <returns>True if the orders are correctly placed</returns>
        public async Task<bool> PlaceBracketOrder(Contract contract, string entryAction, double quantity, double entryOrderPrice, double takePrice, double stopPrice)
        {
            await this.EnsureConnectedAsync();

            // Generate the order IDs
            int entryOrderId = await this.GetNextValidIdAsync();
            var takeProfitOrderId = await this.GetNextValidIdAsync();
            var stopOrderId = await this.GetNextValidIdAsync();

            // Initialize the order
            Order entryOrder = new Order()
            {
                Action = entryAction,
                OrderType = TwsOrderType.Limit,
                TotalQuantity = quantity,
                LmtPrice = entryOrderPrice,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = false,
            };

            Order takeProfit = new Order()
            {
                Action = TwsOrderActions.Reverse(entryAction),
                OrderType = TwsOrderType.Limit,
                TotalQuantity = quantity,
                LmtPrice = takePrice,
                ParentId = entryOrderId,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = false,
            };

            Order stopLoss = new Order()
            {
                Action = TwsOrderActions.Reverse(entryAction),
                OrderType = TwsOrderType.StopLoss,
                TotalQuantity = quantity,
                AuxPrice = stopPrice,
                ParentId = entryOrderId,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = true,
            };

            var entryOrderAckTask = this.PlaceOrderAsync(entryOrderId, contract, entryOrder);
            var takeProfitOrderAckTask = this.PlaceOrderAsync(takeProfitOrderId, contract, takeProfit);
            var stopOrderAckTask = this.PlaceOrderAsync(stopOrderId, contract, stopLoss);
            Task.WaitAll(entryOrderAckTask, takeProfitOrderAckTask, stopOrderAckTask);

            return entryOrderAckTask.Result && takeProfitOrderAckTask.Result && stopOrderAckTask.Result;
        }

        /// <summary>
        /// Places a bracket order with a limit order entry and stop limit exit
        /// </summary>
        /// <param name="contract">The contract</param>
        /// <param name="entryAction">Buy/sell/ssell</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="entryOrderPrice">The limit order entry price</param>
        /// <param name="takePrice">The take profit price</param>
        /// <param name="stopActivationPrice">The stop loss price</param>
        /// <param name="stopLimitPrice">The price to put the limit after the stop activation price is touched</param>
        /// <returns>True if the orders are correctly placed</returns>
        public async Task<bool> PlaceBracketOrder(Contract contract, string entryAction, double quantity, double entryOrderPrice, double takePrice, double stopActivationPrice, double stopLimitPrice)
        {
            await this.EnsureConnectedAsync();

            // Generate the order IDs
            int entryOrderId = await this.GetNextValidIdAsync();
            var takeProfitOrderId = await this.GetNextValidIdAsync();
            var stopOrderId = await this.GetNextValidIdAsync();

            // Initialize the order
            Order entryOrder = new Order()
            {
                Action = entryAction,
                OrderType = TwsOrderType.Limit,
                TotalQuantity = quantity,
                LmtPrice = entryOrderPrice,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = false,
            };

            Order takeProfit = new Order()
            {
                Action = TwsOrderActions.Reverse(entryAction),
                OrderType = TwsOrderType.Limit,
                TotalQuantity = quantity,
                LmtPrice = takePrice,
                ParentId = entryOrderId,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = false,
            };

            Order stopLoss = new Order()
            {
                Action = TwsOrderActions.Reverse(entryAction),
                OrderType = TwsOrderType.StopLimit,
                TotalQuantity = quantity,
                AuxPrice = stopActivationPrice,
                LmtPrice = stopLimitPrice,
                ParentId = entryOrderId,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = true,
            };

            var entryOrderAckTask = this.PlaceOrderAsync(entryOrderId, contract, entryOrder);
            var takeProfitOrderAckTask = this.PlaceOrderAsync(takeProfitOrderId, contract, takeProfit);
            var stopOrderAckTask = this.PlaceOrderAsync(stopOrderId, contract, stopLoss);
            Task.WaitAll(entryOrderAckTask, takeProfitOrderAckTask, stopOrderAckTask);

            return entryOrderAckTask.Result && takeProfitOrderAckTask.Result && stopOrderAckTask.Result;
        }

        /// <summary>
        /// Cancels all orders with the given symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> CancelOrders(string symbol)
        {
            await this.twsControllerBase.EnsureConnectedAsync();

            var openOrders = await this.twsControllerBase.RequestOpenOrders();
            var openOrdersForSymbol = openOrders.Where(orderEvent => orderEvent.Contract.Symbol == symbol).Select(o => o.OrderId).Distinct().ToList();

            if (openOrdersForSymbol.Count == 0)
            {
                return false;
            }

            bool success = true;
            foreach (var openOrder in openOrdersForSymbol)
            {
                try
                {
                    success &= await this.twsControllerBase.CancelOrderAsync(openOrder);
                }
                catch (TwsException)
                {
                }
            }

            return success;
        }

        /// <summary>
        /// Liquidates the position with the given symbol to the given exchange
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="exchange">The exchange to send the order to. Can remove this param?</param>
        /// <returns>True if successful</returns>
        public async Task<bool> LiquidatePosition(string symbol, string exchange)
        {
            await this.twsControllerBase.EnsureConnectedAsync();

            // Close any outstanding orders
            await this.CancelOrders(symbol);

            // Get the number of shares and direction
            var positions = await this.twsControllerBase.RequestPositions();
            var position = positions.Where(p => p.Contract.Symbol == symbol && p.Position != 0).FirstOrDefault();

            if (position == null)
            {
                return false;
            }

            // Use a REL order to exit
            string liquidationOrderDirection = string.Empty;
            if (position.Position > 0)
            {
                liquidationOrderDirection = TwsOrderActions.Sell;
            }

            if (position.Position < 0)
            {
                liquidationOrderDirection = TwsOrderActions.Buy;
            }

            Order order = new Order()
            {
                Action = liquidationOrderDirection,
                OrderType = TwsOrderType.PegToMarket,
                TotalQuantity = Math.Abs(position.Position),
            };

            position.Contract.Exchange = exchange; // TWS does not save the original traded exchange on the contract.

            int liquidationOrderId = await this.twsControllerBase.GetNextValidIdAsync();
            bool success = await this.twsControllerBase.PlaceOrderAsync(liquidationOrderId, position.Contract, order);

            return success;
        }

#pragma warning disable SA1600 // Elements should be documented
        public async Task EnsureConnectedAsync()
        {
            await this.twsControllerBase.EnsureConnectedAsync();
        }

        public async Task DisconnectAsync()
        {
            await this.twsControllerBase.DisconnectAsync();
        }

        public async Task<int> GetNextValidIdAsync()
        {
            return await this.twsControllerBase.GetNextValidIdAsync();
        }

        public async Task<int> GetNextValidIdAsync(CancellationToken cancellationToken)
        {
            return await this.twsControllerBase.GetNextValidIdAsync(cancellationToken);
        }

        public int GetNextRequestId()
        {
            return this.twsControllerBase.GetNextRequestId();
        }

        public async Task<List<OpenOrderEventArgs>> RequestOpenOrders()
        {
            return await this.twsControllerBase.RequestOpenOrders();
        }

        public async Task<List<OpenOrderEventArgs>> RequestOpenOrders(CancellationToken cancellationToken)
        {
            return await this.twsControllerBase.RequestOpenOrders(cancellationToken);
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            return await this.twsControllerBase.CancelOrderAsync(orderId);
        }

        public async Task<bool> CancelOrderAsync(int orderId, CancellationToken cancellationToken)
        {
            return await this.twsControllerBase.CancelOrderAsync(orderId);
        }

        public async Task<bool> PlaceOrderAsync(int orderId, Contract contract, Order order)
        {
            return await this.twsControllerBase.PlaceOrderAsync(orderId, contract, order);
        }

        public async Task<bool> PlaceOrderAsync(int orderId, Contract contract, Order order, CancellationToken cancellationToken)
        {
            return await this.twsControllerBase.PlaceOrderAsync(orderId, contract, order, cancellationToken);
        }

        public async Task<ConcurrentDictionary<string, string>> GetAccountDetailsAsync(string accountId)
        {
            return await this.twsControllerBase.GetAccountDetailsAsync(accountId);
        }

        public async Task<ConcurrentDictionary<string, string>> GetAccountDetailsAsync(string accountId, CancellationToken cancellationToken)
        {
            return await this.GetAccountDetailsAsync(accountId, cancellationToken);
        }

        public async Task<List<ContractDetails>> GetContractAsync(Contract contract)
        {
            return await this.twsControllerBase.GetContractAsync(contract);
        }

        public async Task<List<ExecutionDetailsEventArgs>> RequestExecutions()
        {
            return await this.twsControllerBase.RequestExecutions();
        }

        public async Task<List<HistoricalDataEventArgs>> GetHistoricalDataAsync(Contract contract, DateTime endDateTime, TwsDuration duration, TwsBarSizeSetting barSizeSetting, TwsHistoricalDataRequestType whatToShow)
        {
            return await this.twsControllerBase.GetHistoricalDataAsync(contract, endDateTime, duration, barSizeSetting, whatToShow);
        }

        public void CancelHistoricalData(int requestId)
        {
            this.twsControllerBase.CancelHistoricalData(requestId);
        }

        public async Task<NewsProviderEventArgs> GetNewsProviders()
        {
            return await this.twsControllerBase.GetNewsProviders();
        }

        public async Task<List<PositionStatusEventArgs>> RequestPositions()
        {
            return await this.twsControllerBase.RequestPositions();
        }

        public async Task<List<SecurityDefinitionOptionParameterEventArgs>> RequestSecurityDefinitionOptionParameters(string underlyingSymbol, string exchange, string underlyingSecType, int underlyingConId)
        {
            return await this.twsControllerBase.RequestSecurityDefinitionOptionParameters(underlyingSymbol, exchange, underlyingSecType, underlyingConId);
        }

        public async Task<MarketDataTypeEventArgs> RequestMarketDataTypeAsync(int marketDataType)
        {
            return await this.twsControllerBase.RequestMarketDataTypeAsync(marketDataType);
        }

        public async Task<PnLEventArgs> RequestPnL(string accountCode, string modelCode)
        {
            return await this.twsControllerBase.RequestPnL(accountCode, modelCode);
        }

        public async Task<PnLSingleEventArgs> RequestPnLSingle(string accountCode, string modelCode, int conId)
        {
            return await this.twsControllerBase.RequestPnLSingle(accountCode, modelCode, conId);
        }

        public void CancelAccountDetails(string accountId)
        {
            this.twsControllerBase.CancelAccountDetails(accountId);
        }

        public void CancelPositions()
        {
            this.twsControllerBase.CancelPositions();
        }

        public void CancelPnL(int reqId)
        {
            this.twsControllerBase.CancelPnL(reqId);
        }

        public void CancelPnLSingle(int reqId)
        {
            this.twsControllerBase.CancelPnLSingle(reqId);
        }
#pragma warning restore SA1600 // Elements should be documented
    }
}
