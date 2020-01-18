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
    /// Test the bracket order controller
    /// </summary>
    public class TwsBracketOrderControllerTests
    {
        /// <summary>
        /// Place a bracket order
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task OrderPlacementController_Should_PlaceBracketOrder()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);

            // Initialize the contract to trade
            Contract contract = new Contract()
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
            };

            // Make sure there are some open orders by placing a bracket order
            var twsBracketOrderPlacementController = new TwsBracketOrderPlacementController(connectionController, nextOrderIdController, orderPlacementController);
            bool orderAck = await twsBracketOrderPlacementController.PlaceBracketOrder(contract, TwsOrderActions.Buy, 1, 200, 100);
            orderAck.Should().BeTrue();
        }
    }
}
