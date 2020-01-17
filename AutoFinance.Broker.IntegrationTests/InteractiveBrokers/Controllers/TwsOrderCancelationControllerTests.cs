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
    /// Tests the order cancellation controller against TWS.
    /// </summary>
    public class TwsOrderCancelationControllerTests
    {
        /// <summary>
        /// Tests that orders can be canceled in TWS
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CancelOrder_Should_CancelOrder()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();

            // This should be fixed a bit to be injectable
            // It's a bit dirty because you need to run ConfigureTws before you have access to the client socket and callback handler
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderCancelationController orderCancelationController = new TwsOrderCancelationController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);

            await connectionController.ConnectAsync();

            // Initialize the contract
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Future,
                Symbol = TwsSymbol.Dax,
                Exchange = TwsExchange.Dtb,
                Currency = TwsCurrency.Eur,
                Multiplier = "25",
                LastTradeDateOrContractMonth = "201809"
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
            int orderId = await nextOrderIdController.GetNextValidIdAsync();
            bool orderAcknowledged = await orderPlacementController.PlaceOrderAsync(orderId, contract, order);
            orderAcknowledged.Should().BeTrue();

            // Call the API
            bool cancelationAcknowledged = await orderCancelationController.CancelOrderAsync(orderId);

            // Assert
            cancelationAcknowledged.Should().BeTrue();
        }
    }
}
