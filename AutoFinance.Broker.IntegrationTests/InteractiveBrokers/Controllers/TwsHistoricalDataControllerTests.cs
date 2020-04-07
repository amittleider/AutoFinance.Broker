﻿namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
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
        [Fact(Skip = "Requires market data subscription")]
        public async Task HistoricalDataController_Should_RetrieveHistoricalData()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();

            TwsRequestIdGenerator twsRequestIdGenerator = new TwsRequestIdGenerator();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            TwsHistoricalDataController historicalDataController = new TwsHistoricalDataController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, twsRequestIdGenerator);

            await connectionController.EnsureConnectedAsync();

            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
            };

            var queryTime = DateTime.Now.AddMonths(-6);

            // Call
            List<HistoricalDataEventArgs> historicalDataEvents = await historicalDataController.GetHistoricalDataAsync(contract, queryTime, TwsDuration.OneMonth, TwsBarSizeSetting.OneDay, TwsHistoricalDataRequestType.Midpoint);

            // Assert
            historicalDataEvents.Count.Should().BeGreaterThan(0);
        }
    }
}
