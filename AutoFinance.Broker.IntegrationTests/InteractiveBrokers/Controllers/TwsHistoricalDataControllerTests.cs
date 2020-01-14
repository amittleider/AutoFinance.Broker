// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using AutoFinance.Broker.InteractiveBrokers.Controllers;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using FluentAssertions;
    using IBApi;
    using Xunit;

    /// <summary>
    /// Tests the behavior of the historical data controller against a real TWS instance
    /// </summary>
    public class TwsHistoricalDataControllerTests
    {
        /// <summary>
        /// Ensure that the historical data controller can retrieve data
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task HistoricalDataController_Should_RetrieveHistoricalData()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();

            TwsRequestIdGenerator twsRequestIdGenerator = new TwsRequestIdGenerator();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsHistoricalDataController historicalDataController = new TwsHistoricalDataController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, twsRequestIdGenerator);

            await connectionController.ConnectAsync();

            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Future,
                Symbol = TwsSymbol.Dax,
                Exchange = TwsExchange.Dtb,
                Currency = TwsCurrency.Eur,
                Multiplier = "25",
                LastTradeDateOrContractMonth = "201809"
            };

            string queryTime = DateTime.Now.AddMonths(-6).ToString("yyyyMMdd HH:mm:ss");

            // Call
            List<HistoricalDataEventArgs> historicalDataEvents = await historicalDataController.GetHistoricalDataAsync(contract, queryTime, "1 M", "1 day", "MIDPOINT");

            // Assert
            historicalDataEvents.Count.Should().BeGreaterThan(0);
        }
    }
}
