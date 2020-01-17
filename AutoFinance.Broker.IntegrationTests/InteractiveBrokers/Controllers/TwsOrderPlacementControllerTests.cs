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
    /// </summary>
    public class TwsOrderPlacementControllerTests
    {
        /// <summary>
        /// Test that the order placement controller can successfully place orders to TWS
        /// Note: This test fails awaiting if the market is closed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_PlaceOrderSuccessfully()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();

            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);

            await connectionController.ConnectAsync();

            // Initialize the contract
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Future;
            contract.Symbol = TwsSymbol.Dax;
            contract.Exchange = TwsExchange.Dtb;
            contract.Currency = TwsCurrency.Eur;
            contract.Multiplier = "25";
            contract.LastTradeDateOrContractMonth = "201809";

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
        /// TODO: This test fails awaiting if the market is closed.
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

            await connectionController.ConnectAsync();

            // Initialize the contract
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Future;
            contract.Symbol = TwsSymbol.Dax;
            contract.Exchange = TwsExchange.Dtb;
            contract.Currency = TwsCurrency.Eur;
            contract.Multiplier = "25";
            contract.LastTradeDateOrContractMonth = "201809";

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
    }
}
