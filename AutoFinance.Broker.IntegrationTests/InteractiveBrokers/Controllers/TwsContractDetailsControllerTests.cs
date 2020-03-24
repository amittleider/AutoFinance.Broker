namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
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

            await connectionController.EnsureConnectedAsync();

            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
            };

            // Call
            List<ContractDetails> contractDetails = await contractDetailsController.GetContractAsync(contract);

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

            await connectionController.EnsureConnectedAsync();

            Contract contract = new Contract();
            contract.Symbol = "EUR";
            contract.SecType = "CASH";
            contract.Currency = "GBP";
            contract.Exchange = "IDEALPRO";

            // Call
            List<ContractDetails> contractDetails = await contractDetailsController.GetContractAsync(contract);

            // Assert
            contractDetails.First().Should().NotBeNull();

            // Tear down
            await connectionController.DisconnectAsync();
        }
    }
}
