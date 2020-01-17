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
    /// Tests the TWS EClientSocket communication.
    /// </summary>
    public class TwsContractDetailsControllerTests
    {
        /// <summary>
        /// Test that contract details are correctly returned from the ContractDetailsController.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ContractDetailsController_Should_ReturnValidContractAsync()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();

            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            TwsContractDetailsController contractDetailsController = new TwsContractDetailsController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, new TwsRequestIdGenerator());

            await connectionController.ConnectAsync();

            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Future;
            contract.Symbol = TwsSymbol.Dax;
            contract.Exchange = TwsExchange.Dtb;
            contract.Currency = TwsCurrency.Eur;
            contract.Multiplier = "25";
            contract.LastTradeDateOrContractMonth = "201809";

            // Call
            ContractDetails contractDetails = await contractDetailsController.GetContractAsync(contract);

            // Assert
            contractDetails.Should().NotBeNull();

            // Tear down
            await connectionController.DisconnectAsync();
        }

        /// <summary>
        /// Test that contract details are correctly returned from the ContractDetailsController.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ContractDetailsController_Should_ReturnValidForexContractAsync()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();

            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            TwsContractDetailsController contractDetailsController = new TwsContractDetailsController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, new TwsRequestIdGenerator());

            await connectionController.ConnectAsync();

            Contract contract = new Contract();
            contract.Symbol = "EUR";
            contract.SecType = "CASH";
            contract.Currency = "GBP";
            contract.Exchange = "IDEALPRO";

            // Call
            ContractDetails contractDetails = await contractDetailsController.GetContractAsync(contract);

            // Assert
            contractDetails.Should().NotBeNull();

            // Tear down
            await connectionController.DisconnectAsync();
        }
    }
}
