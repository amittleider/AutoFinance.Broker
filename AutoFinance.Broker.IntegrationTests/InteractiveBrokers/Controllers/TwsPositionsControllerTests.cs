// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using AutoFinance.Broker.InteractiveBrokers.Controllers;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;
    using FluentAssertions;
    using IBApi;
    using Xunit;

    /// <summary>
    /// Tests the controller that retrieves account updates from TWS
    /// </summary>
    public class TwsPositionsControllerTests
    {
        /// <summary>
        /// Test that position events come back from TWS properly
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task PositionsController_Should_ReturnAListOfPositions()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsPositionsController positionsController = new TwsPositionsController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);

            await connectionController.ConnectAsync();

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
            List<PositionStatusEventArgs> positionStatusEvents = await positionsController.RequestPositions();

            // Assert
            positionStatusEvents.Count.Should().BeGreaterOrEqualTo(0);
            PositionStatusEventArgs daxPositions = positionStatusEvents.Where(eventArgs => eventArgs.Contract.Symbol == contract.Symbol).FirstOrDefault();
            daxPositions.Position.Should().BeGreaterOrEqualTo(order.TotalQuantity);

            // Tear down
            await connectionController.DisconnectAsync();
        }
    }
}
