using AutoFinance.Broker.InteractiveBrokers;
using AutoFinance.Broker.InteractiveBrokers.Constants;
using AutoFinance.Broker.InteractiveBrokers.Controllers;
using AutoFinance.Broker.InteractiveBrokers.EventArgs;
using AutoFinance.Broker.InteractiveBrokers.Wrappers;
using FluentAssertions;
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
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Call
            string accountId = "DU1052488";
            ConcurrentDictionary<string, string> accountUpdates = await twsController.GetAccountDetailsAsync(accountId);

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
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
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
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            Contract contract = new Contract();
            contract.Symbol = "EUR";
            contract.SecType = "CASH";
            contract.Currency = "GBP";
            contract.Exchange = "IDEALPRO";

            // Call
            List<ContractDetails> contractDetails = await twsController.GetContractAsync(contract);

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
        public async Task TwsExecutionController_Should_ReturnExecutions()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Create a position
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Future;
            contract.Symbol = TwsSymbol.Dax;
            contract.Exchange = TwsExchange.Dtb;
            contract.Currency = TwsCurrency.Eur;
            contract.Multiplier = "25";
            contract.LastTradeDateOrContractMonth = "201809";

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
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
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
        }

        [Fact]
        public async Task Should_ReturnOpenOrders()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
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
        }

        [Fact]
        public async Task TwsExecutionController_Should_ReturnOpenOrdersTwice()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
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
        }

        /// <summary>
        /// Tests that orders can be canceled in TWS
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CancelOrder_Should_CancelOrder()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
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
        }

        /// <summary>
        /// Test that the order placement controller can successfully place orders to TWS
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_PlaceOrderSuccessfully()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
            ITwsControllerBase twsController = twsObjectFactory.TwsControllerBase;

            await twsController.EnsureConnectedAsync();

            // Initialize the contract
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Future;
            contract.Symbol = TwsSymbol.Dax;
            contract.Exchange = TwsExchange.Dtb;
            contract.Currency = TwsCurrency.Eur;
            contract.Multiplier = "25";
            contract.LastTradeDateOrContractMonth = "202009";

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
            // It appears something is wrong with the disconnection API, which could be screwing up all the integration tests if you don't run them individually.
            ////await connectionController.DisconnectAsync();
        }

        /// <summary>
        /// Test that the order placement controller can successfully place orders to TWS
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_PlaceTwoOrdersSuccessfully()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
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
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
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
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
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
            var orderAck = await twsController.PlaceOrderAsync(orderId, contract, order);

            // Assert
            orderAck.Should().BeFalse();

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
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
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
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
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
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
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
            List<PositionStatusEventArgs> positionStatusEvents = await twsController.RequestPositions();

            // Assert
            positionStatusEvents.Count.Should().BeGreaterOrEqualTo(0);
            PositionStatusEventArgs daxPositions = positionStatusEvents.Where(eventArgs => eventArgs.Contract.Symbol == contract.Symbol).FirstOrDefault();
            daxPositions.Position.Should().BeGreaterOrEqualTo(order.TotalQuantity);

            // Tear down
            await twsController.DisconnectAsync();
        }

        [Fact]
        public async Task Should_GetOptionsContracts()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
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
    }
}
