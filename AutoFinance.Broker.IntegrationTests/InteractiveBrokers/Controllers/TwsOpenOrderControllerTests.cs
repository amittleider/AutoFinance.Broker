namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
{
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using AutoFinance.Broker.InteractiveBrokers.Controllers;
    using FluentAssertions;
    using IBApi;
    using Xunit;

    public class TwsOpenOrderControllerTests
    {
        /// <summary>
        /// Request open orders
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Should_GetOpenOrders()
        {
            // Setup
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);

            await connectionController.EnsureConnectedAsync();

            // Initialize the contract
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
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
            var twsOpenOrderController = new TwsOpenOrderController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);

            // Call
            var orders = await twsOpenOrderController.GetOpenOrders();
            orders.Count.Should().BeGreaterOrEqualTo(3);
        }
    }
}
