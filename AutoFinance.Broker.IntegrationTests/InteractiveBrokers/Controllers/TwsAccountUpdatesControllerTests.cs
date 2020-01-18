// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers;
    using AutoFinance.Broker.InteractiveBrokers.Controllers;
    using FluentAssertions;
    using Xunit;

    /// <summary>
    /// Tests the controller that retrieves account updates from TWS
    /// </summary>
    public class TwsAccountUpdatesControllerTests
    {
        /// <summary>
        /// Test that contract details are correctly returned from the ContractDetailsController.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task AccountUpdatesController_Should_ReturnInformation()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            TwsAccountUpdatesController accountUpdatesController = new TwsAccountUpdatesController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            await connectionController.EnsureConnectedAsync();

            // Call
            string accountId = "DU1052488";
            ConcurrentDictionary<string, string> accountUpdates = await accountUpdatesController.GetAccountDetailsAsync(accountId);

            // Assert
            accountUpdates.Count.Should().BeGreaterThan(0);

            // Tear down
            await connectionController.DisconnectAsync();
        }
    }
}
