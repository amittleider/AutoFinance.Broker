namespace AutoFinance.Broker.UnitTests.InteractiveBrokers
{
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using AutoFinance.Broker.InteractiveBrokers.Controllers;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;
    using FluentAssertions;
    using IBApi;
    using Moq;
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
        public async Task ContractDetailsController_Should_ReturnValidContractAsync_UnderNormalCircumstances()
        {
            // Setup
            // Define the contract input to the contract details controller
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Future;
            contract.Symbol = TwsSymbol.Dax;
            contract.Exchange = TwsExchange.Dtb;
            contract.Currency = TwsCurrency.Eur;
            contract.Multiplier = "25";
            contract.LastTradeDateOrContractMonth = "201806";

            // The following ContractDetails is the output of the call.
            ContractDetails expectedContractDetails = new ContractDetails();

            // Create a fake request Id generator which always returns a request Id of 1
            int fakeRequestId = 1;
            Mock<ITwsRequestIdGenerator> mockTwsRequestIdGenerator = new Mock<ITwsRequestIdGenerator>();
            mockTwsRequestIdGenerator.Setup(mock => mock.GetNextRequestId()).Returns(fakeRequestId);

            // Mock the callback handler, this one is a bit complex thanks to Tws APIs
            // It is saying that at any point that TWSCallbackHandler.contractDetails is called, propagate a new event through the TwsCallbackHandler.ContractDetailsEvent event.
            Mock<ITwsCallbackHandler> mockTwsCallbackHandler = new Mock<ITwsCallbackHandler>();
            mockTwsCallbackHandler.Setup(mock => mock.contractDetails(It.IsAny<int>(), It.IsAny<ContractDetails>())).Callback<int, ContractDetails>((requestId, contractDetails) =>
            {
                mockTwsCallbackHandler.Raise(mock => mock.ContractDetailsEvent += null, null, new ContractDetailsEventArgs(requestId, contractDetails));
            });

            mockTwsCallbackHandler.Setup(mock => mock.contractDetailsEnd(It.IsAny<int>())).Callback<int>((requestId) =>
            {
                mockTwsCallbackHandler.Raise(mock => mock.ContractDetailsEndEvent += null, null, new ContractDetailsEndEventArgs(requestId));
            });

            // Mock the client socket
            Mock<ITwsClientSocket> mockTwsClientSocket = new Mock<ITwsClientSocket>();
            mockTwsClientSocket.Setup(mock => mock.ReqContractDetails(fakeRequestId, contract)).Callback(
                () =>
                {
                    mockTwsCallbackHandler.Object.contractDetails(fakeRequestId, expectedContractDetails);
                    mockTwsCallbackHandler.Object.contractDetailsEnd(fakeRequestId);
                });

            TwsContractDetailsController contractDetailsController = new TwsContractDetailsController(mockTwsClientSocket.Object, mockTwsCallbackHandler.Object, mockTwsRequestIdGenerator.Object);

            // Call
            ContractDetails actualContractDetails = await contractDetailsController.GetContractAsync(contract);

            // Assert
            actualContractDetails.Should().Be(expectedContractDetails);
        }
    }
}
