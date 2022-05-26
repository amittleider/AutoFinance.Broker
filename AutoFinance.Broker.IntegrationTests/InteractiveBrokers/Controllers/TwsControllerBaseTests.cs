using AutoFinance.Broker.InteractiveBrokers;
using AutoFinance.Broker.InteractiveBrokers.Constants;
using AutoFinance.Broker.InteractiveBrokers.Controllers;
using AutoFinance.Broker.InteractiveBrokers.EventArgs;
using AutoFinance.Broker.InteractiveBrokers.Exceptions;
using AutoFinance.Broker.InteractiveBrokers.Wrappers;
using FluentAssertions;
using FluentAssertions.Common;
using IBApi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
{
    public class TwsControllerBaseTests
    {
        /// <summary>
        /// Test that contract details are correctly returned from the ContractDetailsController.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task AccountUpdatesController_Should_ReturnInformation()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Call
            string accountId = "DU1052488";
            ConcurrentDictionary<string, string> accountUpdates = twsController.GetAccountDetailsAsync(accountId).ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert
            accountUpdates.Count.Should().BeGreaterThan(0);

            // Tear down
            await twsController.DisconnectAsync();
        }

        /// <summary>
        /// Test that contract details are correctly returned from the ContractDetailsController.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ContractDetailsController_Should_ReturnValidContractAsync()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
            };

            // Call
            List<ContractDetails> contractDetails = await twsController.GetContractAsync(contract);

            // Assert
            contractDetails.Should().NotBeNull();

            // Tear down
            await twsController.DisconnectAsync();
        }

        /// <summary>
        /// Test that contract details are correctly returned from the ContractDetailsController.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ContractDetailsController_Should_ReturnValidForexContractAsync()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            Contract contract = new Contract();
            contract.Symbol = "EUR";
            contract.SecType = "CASH";
            contract.Currency = "GBP";
            contract.Exchange = "IDEALPRO";

            // Call
            List<ContractDetails> contractDetails = twsController.GetContractAsync(contract).ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert
            contractDetails.First().Should().NotBeNull();

            // Tear down
            await twsController.DisconnectAsync();
        }

        /// <summary>
        /// Tests that the execution controller properly retreives the executions
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Should_PlaceOrder()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Create a position
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
            };

            Order order = new Order
            {
                Action = "BUY",
                OrderType = "MKT",
                TotalQuantity = 1
            };

            int orderId = await twsController.GetNextValidIdAsync();
            bool successfullyPlaced = await twsController.PlaceOrderAsync(orderId, contract, order);
            Thread.Sleep(1000); // TWS takes some time to put the order in the portfolio. Wait for it.

            // Call
            List<ExecutionDetailsEventArgs> executionDetailEvents = await twsController.RequestExecutions();

            // Assert
            executionDetailEvents.Count.Should().BeGreaterOrEqualTo(0);

            // Tear down
            await twsController.DisconnectAsync();
        }

        /// <summary>
        /// Ensure that the historical data controller can retrieve data
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact(Skip = "Requires market data subscription")]
        public async Task HistoricalDataController_Should_RetrieveHistoricalData()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
            };

            var queryTime = DateTime.Now.AddMonths(-6);

            // Call
            List<HistoricalDataEventArgs> historicalDataEvents = await twsController.GetHistoricalDataAsync(contract, queryTime, TwsDuration.OneMonth, TwsBarSizeSetting.OneDay, TwsHistoricalDataRequestType.Midpoint);

            // Assert
            historicalDataEvents.Count.Should().BeGreaterThan(0);

            await twsController.DisconnectAsync();
        }

        [Fact]
        public async Task Should_ReturnOpenOrders()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Create a position
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Stock;
            contract.Symbol = "MSFT";
            contract.Exchange = TwsExchange.Smart;
            contract.Currency = TwsCurrency.Usd;
            contract.PrimaryExch = TwsExchange.Island;

            Order order = new Order
            {
                Action = "BUY",
                OrderType = "LMT",
                TotalQuantity = 1,
                LmtPrice = 1,
            };

            int orderId = await twsController.GetNextValidIdAsync();
            bool successfullyPlaced = await twsController.PlaceOrderAsync(orderId, contract, order);
            Thread.Sleep(1000); // TWS takes some time to put the order in the portfolio. Wait for it.

            var openOrders = await twsController.RequestOpenOrders();
            openOrders.Count.Should().BeGreaterOrEqualTo(1);

            var msftOrders = openOrders.Where(orderEvent => orderEvent.Contract.Symbol == "MSFT").ToList();
            msftOrders.Count.Should().BeGreaterOrEqualTo(1);

            await twsController.DisconnectAsync();
        }

        [Fact]
        public async Task TwsExecutionController_Should_ReturnOpenOrdersTwice()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Create a position
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Stock;
            contract.Symbol = "MSFT";
            contract.Exchange = TwsExchange.Smart;
            contract.Currency = TwsCurrency.Usd;
            contract.PrimaryExch = TwsExchange.Island;

            Order order = new Order
            {
                Action = "BUY",
                OrderType = "LMT",
                TotalQuantity = 1,
                LmtPrice = 1,
            };

            int orderId = await twsController.GetNextValidIdAsync();
            bool successfullyPlaced = await twsController.PlaceOrderAsync(orderId, contract, order);
            Thread.Sleep(1000); // TWS takes some time to put the order in the portfolio. Wait for it.

            var openOrders = await twsController.RequestOpenOrders();
            openOrders.Count.Should().BeGreaterOrEqualTo(1);

            Thread.Sleep(5005);

            var openOrders2 = await twsController.RequestOpenOrders();
            openOrders2.Count.Should().BeGreaterOrEqualTo(1);

            await twsController.DisconnectAsync();
        }

        /// <summary>
        /// Tests that orders can be canceled in TWS
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CancelOrder_Should_CancelOrder()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Initialize the contract
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
            };

            // Initialize the order
            Order order = new Order
            {
                Action = "BUY",
                OrderType = "LMT",
                TotalQuantity = 1,
                LmtPrice = 1,
            };

            // Place an order
            int orderId = await twsController.GetNextValidIdAsync();
            bool orderAcknowledged = await twsController.PlaceOrderAsync(orderId, contract, order);
            orderAcknowledged.Should().BeTrue();

            // Call the API
            bool cancelationAcknowledged = await twsController.CancelOrderAsync(orderId);

            // Assert
            cancelationAcknowledged.Should().BeTrue();

            await twsController.DisconnectAsync();
        }

        [Fact]
        public async Task OrderPlacementController_Should_PlaceOrderSuccessfully2()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Initialize the contract
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "GRWG",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
                Currency = TwsCurrency.Usd,
            };

            // Initialize the order
            Order order = new Order
            {
                Action = "BUY",
                OrderType = "MKT",
                TotalQuantity = 1
            };

            // Call the API
            int orderId = await twsController.GetNextValidIdAsync();
            bool successfullyPlaced = await twsController.PlaceOrderAsync(orderId, contract, order);

            // Assert
            successfullyPlaced.Should().BeTrue();

            // Tear down
            await twsController.DisconnectAsync();
        }

        /// <summary>
        /// Test that the order placement controller can successfully place orders to TWS
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_PlaceTwoOrdersSuccessfully()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Initialize the contract
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
            };

            // Initialize the order
            Order order = new Order
            {
                Action = "BUY",
                OrderType = "MKT",
                TotalQuantity = 1
            };

            // Call the API
            int orderId = await twsController.GetNextValidIdAsync();
            var firstOrderAcknowledgedTask = twsController.PlaceOrderAsync(orderId, contract, order);
            orderId = await twsController.GetNextValidIdAsync();
            var secondOrderAcknowledgedTask = twsController.PlaceOrderAsync(orderId, contract, order);

            Task.WaitAll(firstOrderAcknowledgedTask, secondOrderAcknowledgedTask);

            // Assert
            firstOrderAcknowledgedTask.Result.Should().BeTrue();
            secondOrderAcknowledgedTask.Result.Should().BeTrue();

            // Tear down
            await twsController.DisconnectAsync();
        }

        /// <summary>
        /// Test that the order placement controller can successfully place a stock order
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_PlaceStockOrderSuccessfully()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Initialize the contract
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Stock;
            contract.Symbol = "MSFT";
            contract.Exchange = TwsExchange.Smart;
            contract.PrimaryExch = TwsExchange.Island;

            // Initialize the order
            Order order = new Order
            {
                Action = TwsOrderActions.Buy,
                OrderType = "MKT",
                TotalQuantity = 1
            };

            // Call the API
            int orderId = await twsController.GetNextValidIdAsync();
            var firstOrderAcknowledgedTask = twsController.PlaceOrderAsync(orderId, contract, order);
            orderId = await twsController.GetNextValidIdAsync();
            var secondOrderAcknowledgedTask = twsController.PlaceOrderAsync(orderId, contract, order);

            Task.WaitAll(firstOrderAcknowledgedTask, secondOrderAcknowledgedTask);

            // Assert
            firstOrderAcknowledgedTask.Result.Should().BeTrue();
            secondOrderAcknowledgedTask.Result.Should().BeTrue();

            // Tear down
            await twsController.DisconnectAsync();
        }

        /// <summary>
        /// Handle error callback with bad Peg to Midpoint Order
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_HandleErrorCallback()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Initialize the contract
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Stock;
            contract.Symbol = "MSFT";
            contract.Exchange = TwsExchange.Island;
            contract.PrimaryExch = TwsExchange.Island;

            // Initialize the order
            Order order = new Order()
            {
                Action = TwsOrderActions.Buy,
                OrderType = "PEG MID",
                TotalQuantity = 1,
                LmtPrice = 200,
                AuxPrice = 0,
            };

            // Call the API
            int orderId = await twsController.GetNextValidIdAsync();
            Action orderFunc = () => twsController.PlaceOrderAsync(orderId, contract, order).ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert
            orderFunc.Should().Throw<TwsException>();

            // Tear down
            await twsController.DisconnectAsync();
        }

        /// <summary>
        /// Place a peg to midpoint order
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_PlacePegToMidpointOrder()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Initialize the contract
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Stock;
            contract.Symbol = "MSFT";
            contract.Exchange = TwsExchange.Smart;
            contract.PrimaryExch = TwsExchange.Island;

            // Initialize the order
            Order order = new Order()
            {
                Action = TwsOrderActions.Buy,
                OrderType = "REL",
                TotalQuantity = 1,
                LmtPrice = 166,
                AuxPrice = 0.1,
            };

            // Call the API
            int orderId = await twsController.GetNextValidIdAsync();
            var orderAck = await twsController.PlaceOrderAsync(orderId, contract, order);

            // Assert
            orderAck.Should().BeTrue();

            // Tear down
            await twsController.DisconnectAsync();
        }

        /// <summary>
        /// Place a bracket order
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_PlaceBracketOrder()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Initialize the contract
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Stock;
            contract.Symbol = "MSFT";
            contract.Exchange = TwsExchange.Smart;
            contract.PrimaryExch = TwsExchange.Island;

            int entryOrderId = await twsController.GetNextValidIdAsync();
            var takeProfitOrderId = await twsController.GetNextValidIdAsync();
            var stopOrderId = await twsController.GetNextValidIdAsync();

            // Initialize the order
            Order entryOrder = new Order()
            {
                Action = TwsOrderActions.Buy,
                OrderType = TwsOrderType.Market,
                TotalQuantity = 1,
                Transmit = false
            };

            Order takeProfit = new Order()
            {
                Action = TwsOrderActions.Sell,
                OrderType = TwsOrderType.Limit,
                TotalQuantity = 1,
                LmtPrice = 190,
                ParentId = entryOrderId,
                Transmit = false,
            };

            Order stopLoss = new Order()
            {
                Action = TwsOrderActions.Sell,
                OrderType = TwsOrderType.StopLoss,
                TotalQuantity = 1,
                AuxPrice = 100,
                ParentId = entryOrderId,
                Transmit = true,
            };

            // Call the API
            var entryOrderAckTask = twsController.PlaceOrderAsync(entryOrderId, contract, entryOrder);
            var takeProfitOrderAckTask = twsController.PlaceOrderAsync(takeProfitOrderId, contract, takeProfit);
            var stopOrderAckTask = twsController.PlaceOrderAsync(stopOrderId, contract, stopLoss);
            Task.WaitAll(entryOrderAckTask, takeProfitOrderAckTask, stopOrderAckTask);

            // Assert
            entryOrderAckTask.Result.Should().BeTrue();
            takeProfitOrderAckTask.Result.Should().BeTrue();
            stopOrderAckTask.Result.Should().BeTrue();

            // Tear down
            await twsController.DisconnectAsync();
        }

        /// <summary>
        /// Test that position events come back from TWS properly
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task PositionsController_Should_ReturnAListOfPositions()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Create a position
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
            };

            Order order = new Order
            {
                Action = "BUY",
                OrderType = "MKT",
                TotalQuantity = 1
            };

            int orderId = await twsController.GetNextValidIdAsync();
            bool successfullyPlaced = twsController.PlaceOrderAsync(orderId, contract, order).ConfigureAwait(false).GetAwaiter().GetResult();
            Thread.Sleep(1000); // TWS takes some time to put the order in the portfolio. Wait for it.

            // Call
            List<PositionStatusEventArgs> positionStatusEvents = twsController.RequestPositions().ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert
            positionStatusEvents.Count.Should().BeGreaterOrEqualTo(0);
            PositionStatusEventArgs searchedPositions = positionStatusEvents.Where(eventArgs => eventArgs.Contract.Symbol == contract.Symbol).FirstOrDefault();
            searchedPositions.Position.Should().BeGreaterOrEqualTo(order.TotalQuantity);

            // Tear down
            await twsController.DisconnectAsync();
        }

        [Fact]
        public async Task Should_GetOptionsContracts()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
            };

            // Get the contract details of the STOCK so that you can find the underlying security ID, required for the security definitions call.
            var contractDetails = await twsController.GetContractAsync(contract);
            var securityDefinitions = await twsController.RequestSecurityDefinitionOptionParameters("MSFT", "", "STK", contractDetails.First().Contract.ConId);

            securityDefinitions.Count.Should().BeGreaterThan(1);

            await twsController.DisconnectAsync();

            ////// If you want, you can request the contract details from this info or get historical data for it
            ////Contract option = new Contract()
            ////{
            ////    SecType = TwsContractSecType.Option,
            ////    Symbol = "MSFT",
            ////    Exchange = "SMART",
            ////    Strike = 150,
            ////    LastTradeDateOrContractMonth = securityDefinitions[0].Expirations.First(), // March 27, 20
            ////    Right = "C",
            ////    Multiplier = securityDefinitions[0].Multiplier,
            ////    Currency = TwsCurrency.Usd,
            ////};

            ////var optionContractDetails = await twsContractDetailsController.GetContractAsync(option);
            ////var queryTime = DateTime.Now;
            ////List<HistoricalDataEventArgs> historicalDataEvents = await twsHistoricalDataController.GetHistoricalDataAsync(option, queryTime, TwsDuration.OneMonth, TwsBarSizeSetting.OneMinute, TwsHistoricalDataRequestType.Trades);
        }

        /// <summary>
        /// Test that market data type events come back from TWS properly
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MarketDataController_Should_ReturnMarketDataType()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            MarketDataTypeEventArgs marketDataTypeEventArgs = null;
            TickPriceEventArgs tickPriceEventArgs = null;
            twsObjectFactory.TwsCallbackHandler.MarketDataTypeEvent +=
                (sender, args) => { marketDataTypeEventArgs = args; };
            twsObjectFactory.TwsCallbackHandler.TickPriceEvent +=
                (sender, args) => { tickPriceEventArgs = args; };

            // Only real-time data provided for this contract
            var contract = new Contract()
            {
                Symbol = TwsCurrency.Eur,
                Exchange = TwsExchange.Idealpro,
                SecType = TwsContractSecType.Cash,
                Currency = TwsCurrency.Usd
            };

            var marketDataResult = await twsObjectFactory.TwsControllerBase.RequestMarketDataAsync(contract, "233", false, false, null);

            marketDataResult.Should().NotBeNull();
            tickPriceEventArgs.Should().NotBeNull();
            tickPriceEventArgs.TickerId.Should().IsSameOrEqualTo(marketDataResult.TickerId);

            marketDataTypeEventArgs.Should().NotBeNull();
            marketDataTypeEventArgs.MarketDataType.Should().Be(1);
        }

        /// <summary>
        /// Test that market data type events come back from TWS properly
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MarketDataTypeController_Should_ReturnMarketDataType()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            MarketDataTypeEventArgs marketDataTypeEventArgs = null;
            TickPriceEventArgs tickPriceEventArgs = null;
            twsObjectFactory.TwsCallbackHandler.MarketDataTypeEvent +=
                (sender, args) => { marketDataTypeEventArgs = args; };
            twsObjectFactory.TwsCallbackHandler.TickPriceEvent +=
                (sender, args) => { tickPriceEventArgs = args; };

            // This contract provide delayed data when requested
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
            };

            // Request delayed data feed
            twsObjectFactory.TwsControllerBase.RequestMarketDataType(3);
            var marketDataResult = await twsObjectFactory.TwsControllerBase.RequestMarketDataAsync(contract, "233", false, false, null);

            marketDataResult.Should().NotBeNull();
            tickPriceEventArgs.Should().NotBeNull();
            tickPriceEventArgs.TickerId.Should().IsSameOrEqualTo(marketDataResult.TickerId);

            marketDataTypeEventArgs.Should().NotBeNull();
            marketDataTypeEventArgs.MarketDataType.Should().Be(3);
        }

        /// <summary>
        /// Test that pnl type events come back from TWS properly
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task PnLController_Should_ReturnPnL()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            PnLEventArgs pnlEventArgs = null;
            twsObjectFactory.TwsCallbackHandler.PnLEvent +=
                (sender, args) => { pnlEventArgs = args; };

            var pnlResult = await twsObjectFactory.TwsControllerBase.RequestPnL("DU1052488", "");

            pnlEventArgs.Should().NotBeNull();
            pnlResult.Should().NotBeNull();
            pnlEventArgs.RequestId.Should().IsSameOrEqualTo(pnlResult.RequestId);
        }

        /// <summary>
        /// Test that cancel pnl controller stops pnl event from TWS properly
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CancelPnLController_Should_StopPnLEvent()
        {
            int waitDelayInMs = 5000;

            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            PnLEventArgs pnlEventArgs = null;
            DateTime pnlEventTriggerDateTime = DateTime.MaxValue;
            twsObjectFactory.TwsCallbackHandler.PnLEvent +=
                (sender, args) => { pnlEventArgs = args; pnlEventTriggerDateTime = DateTime.Now; };

            var pnlResult = await twsObjectFactory.TwsControllerBase.RequestPnL("DU1052488", "");

            pnlEventArgs.Should().NotBeNull();
            pnlResult.Should().NotBeNull();
            pnlEventArgs.RequestId.Should().IsSameOrEqualTo(pnlResult.RequestId);

            twsObjectFactory.TwsControllerBase.CancelPnL(pnlResult.RequestId);

            await Task.Delay(waitDelayInMs);

            pnlEventTriggerDateTime.Ticks.Should().BeLessThan(DateTime.Now.AddMilliseconds(-waitDelayInMs).Ticks);
        }

        /// <summary>
        /// Test that pnl single type events come back from TWS properly
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task PnLSingleController_Should_ReturnPnLSingle()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            PnLSingleEventArgs pnlSingleEventArgs = null;
            twsObjectFactory.TwsCallbackHandler.PnLSingleEvent +=
                (sender, args) => { pnlSingleEventArgs = args; };

            // Create a position
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
            };

            Order order = new Order
            {
                Action = "BUY",
                OrderType = "MKT",
                TotalQuantity = 1,               
            };

            int orderId = await twsController.GetNextValidIdAsync();
            bool successfullyPlaced = await twsController.PlaceOrderAsync(orderId, contract, order);
            Thread.Sleep(1000); // TWS takes some time to put the order in the portfolio. Wait for it.

            // Call
            List<ExecutionDetailsEventArgs> executionDetailEvents = await twsController.RequestExecutions();

            // Assert
            executionDetailEvents.Count.Should().BeGreaterOrEqualTo(0);

            var pnlSingleResult = await twsObjectFactory.TwsControllerBase.RequestPnLSingle(
                "DU1052488",
                "",
                executionDetailEvents.First().Contract.ConId);

            pnlSingleEventArgs.Should().NotBeNull();
            pnlSingleResult.Should().NotBeNull();
            pnlSingleEventArgs.RequestId.Should().IsSameOrEqualTo(pnlSingleResult.RequestId);
        }

        /// <summary>
        /// Test that cancel pnl single controller stops pnl single event from TWS properly
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CancelPnLSingleController_Should_StopPnLEventSingle()
        {
            int waitDelayInMs = 5000;

            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();


            PnLSingleEventArgs pnlSingleEventArgs = null;
            DateTime pnlEventTriggerDateTime = DateTime.MaxValue;
            twsObjectFactory.TwsCallbackHandler.PnLSingleEvent +=
                (sender, args) => { pnlSingleEventArgs = args; pnlEventTriggerDateTime = DateTime.Now; };

            // Create a position
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
            };

            Order order = new Order
            {
                Action = "BUY",
                OrderType = "MKT",
                TotalQuantity = 1,
            };

            int orderId = await twsController.GetNextValidIdAsync();
            bool successfullyPlaced = twsController.PlaceOrderAsync(orderId, contract, order).ConfigureAwait(false).GetAwaiter().GetResult();
            Thread.Sleep(1000); // TWS takes some time to put the order in the portfolio. Wait for it.

            // Call
            List<ExecutionDetailsEventArgs> executionDetailEvents = twsController.RequestExecutions().ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert
            executionDetailEvents.Count.Should().BeGreaterOrEqualTo(0);

            var pnlSingleResult = twsObjectFactory.TwsControllerBase.RequestPnLSingle(
                "DU1052488",
                "",
                executionDetailEvents.First().Contract.ConId).ConfigureAwait(false).GetAwaiter().GetResult();

            pnlSingleEventArgs.Should().NotBeNull();
            pnlSingleResult.Should().NotBeNull();
            pnlSingleEventArgs.RequestId.Should().IsSameOrEqualTo(pnlSingleResult.RequestId);

            twsObjectFactory.TwsControllerBase.CancelPnLSingle(pnlSingleResult.RequestId);

            await Task.Delay(waitDelayInMs);

            pnlEventTriggerDateTime.Ticks.Should().BeLessThan(DateTime.Now.AddMilliseconds(-waitDelayInMs).Ticks);
        }
    }
}
