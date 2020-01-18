// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using AutoFinance.Broker.InteractiveBrokers.Controllers;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using FluentAssertions;
    using IBApi;
    using Xunit;

    /// <summary>
    /// Tests the Tws Execution Controller
    /// </summary>
    public class TwsExecutionControllersTests
    {
        /// <summary>
        /// Tests that the execution controller properly retreives the executions
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TwsExecutionController_Should_ReturnExecutions()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsRequestIdGenerator twsRequestIdGenerator = new TwsRequestIdGenerator();
            TwsExecutionController executionController = new TwsExecutionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, twsRequestIdGenerator);

            await connectionController.EnsureConnectedAsync();

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

            int orderId = await nextOrderIdController.GetNextValidIdAsync();
            bool successfullyPlaced = await orderPlacementController.PlaceOrderAsync(orderId, contract, order);
            Thread.Sleep(1000); // TWS takes some time to put the order in the portfolio. Wait for it.

            // Call
            List<ExecutionDetailsEventArgs> executionDetailEvents = await executionController.RequestExecutions();

            // Assert
            executionDetailEvents.Count.Should().BeGreaterOrEqualTo(0);

            // Tear down
            await connectionController.DisconnectAsync();
        }
    }
}
