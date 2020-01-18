// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
{
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using AutoFinance.Broker.InteractiveBrokers.Controllers;
    using FluentAssertions;
    using IBApi;
    using Xunit;

    /// <summary>
    /// Tests that the order placement controller can send orders to TWS.
    /// Note: These tests fail if the market is closed.
    /// </summary>
    public class TwsOrderPlacementControllerTests
    {
        /// <summary>
        /// Test that the order placement controller can successfully place orders to TWS
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_PlaceOrderSuccessfully()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();

            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 7);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);

            await connectionController.EnsureConnectedAsync();

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
            int orderId = await nextOrderIdController.GetNextValidIdAsync();
            bool successfullyPlaced = await orderPlacementController.PlaceOrderAsync(orderId, contract, order);

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
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();

            // This should be fixed a bit to be injectable
            // It's a bit dirty because you need to run ConfigureTws before you have access to the client socket and callback handler
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);

            await connectionController.EnsureConnectedAsync();

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
            int orderId = await nextOrderIdController.GetNextValidIdAsync();
            var firstOrderAcknowledgedTask = orderPlacementController.PlaceOrderAsync(orderId, contract, order);
            orderId = await nextOrderIdController.GetNextValidIdAsync();
            var secondOrderAcknowledgedTask = orderPlacementController.PlaceOrderAsync(orderId, contract, order);

            Task.WaitAll(firstOrderAcknowledgedTask, secondOrderAcknowledgedTask);

            // Assert
            firstOrderAcknowledgedTask.Result.Should().BeTrue();
            secondOrderAcknowledgedTask.Result.Should().BeTrue();

            // Tear down
            await connectionController.DisconnectAsync();
        }

        /// <summary>
        /// Test that the order placement controller can successfully place a stock order
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_PlaceStockOrderSuccessfully()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();

            // This should be fixed a bit to be injectable
            // It's a bit dirty because you need to run ConfigureTws before you have access to the client socket and callback handler
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);

            await connectionController.EnsureConnectedAsync();

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
            int orderId = await nextOrderIdController.GetNextValidIdAsync();
            var firstOrderAcknowledgedTask = orderPlacementController.PlaceOrderAsync(orderId, contract, order);
            orderId = await nextOrderIdController.GetNextValidIdAsync();
            var secondOrderAcknowledgedTask = orderPlacementController.PlaceOrderAsync(orderId, contract, order);

            Task.WaitAll(firstOrderAcknowledgedTask, secondOrderAcknowledgedTask);

            // Assert
            firstOrderAcknowledgedTask.Result.Should().BeTrue();
            secondOrderAcknowledgedTask.Result.Should().BeTrue();

            // Tear down
            await connectionController.DisconnectAsync();
        }

        /// <summary>
        /// Handle error callback with bad Peg to Midpoint Order
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_HandleErrorCallback()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();

            // This should be fixed a bit to be injectable
            // It's a bit dirty because you need to run ConfigureTws before you have access to the client socket and callback handler
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);

            await connectionController.EnsureConnectedAsync();

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
            int orderId = await nextOrderIdController.GetNextValidIdAsync();
            var orderAck = await orderPlacementController.PlaceOrderAsync(orderId, contract, order);

            // Assert
            orderAck.Should().BeFalse();

            // Tear down
            await connectionController.DisconnectAsync();
        }

        /// <summary>
        /// Place a peg to midpoint order
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_PlacePegToMidpointOrder()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();

            // This should be fixed a bit to be injectable
            // It's a bit dirty because you need to run ConfigureTws before you have access to the client socket and callback handler
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);

            await connectionController.EnsureConnectedAsync();

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
            int orderId = await nextOrderIdController.GetNextValidIdAsync();
            var orderAck = await orderPlacementController.PlaceOrderAsync(orderId, contract, order);

            // Assert
            orderAck.Should().BeTrue();

            // Tear down
            await connectionController.DisconnectAsync();
        }

        /// <summary>
        /// Place a bracket order
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_PlaceBracketOrder()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);

            await connectionController.EnsureConnectedAsync();

            // Initialize the contract
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Stock;
            contract.Symbol = "MSFT";
            contract.Exchange = TwsExchange.Smart;
            contract.PrimaryExch = TwsExchange.Island;

            int entryOrderId = await nextOrderIdController.GetNextValidIdAsync();
            var takeProfitOrderId = await nextOrderIdController.GetNextValidIdAsync();
            var stopOrderId = await nextOrderIdController.GetNextValidIdAsync();

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
            var entryOrderAckTask = orderPlacementController.PlaceOrderAsync(entryOrderId, contract, entryOrder);
            var takeProfitOrderAckTask = orderPlacementController.PlaceOrderAsync(takeProfitOrderId, contract, takeProfit);
            var stopOrderAckTask = orderPlacementController.PlaceOrderAsync(stopOrderId, contract, stopLoss);
            Task.WaitAll(entryOrderAckTask, takeProfitOrderAckTask, stopOrderAckTask);

            // Assert
            entryOrderAckTask.Result.Should().BeTrue();
            takeProfitOrderAckTask.Result.Should().BeTrue();
            stopOrderAckTask.Result.Should().BeTrue();

            // Tear down
            await connectionController.DisconnectAsync();
        }
    }
}
