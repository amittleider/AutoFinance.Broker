using AutoFinance.Broker.InteractiveBrokers;
using AutoFinance.Broker.InteractiveBrokers.Constants;
using AutoFinance.Broker.InteractiveBrokers.Controllers;
using FluentAssertions;
using IBApi;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
{
    public class TwsOpenOrdersControllerTests
    {
        [Fact]
        public async Task TwsExecutionController_Should_ReturnOpenOrders()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            TwsOpenOrdersController twsOpenOrdersController = new TwsOpenOrdersController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsRequestIdGenerator twsRequestIdGenerator = new TwsRequestIdGenerator();
            TwsExecutionController executionController = new TwsExecutionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, twsRequestIdGenerator);

            await connectionController.EnsureConnectedAsync();

            // Create a position
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Stock;
            contract.Symbol = "MSFT";
            contract.Exchange = TwsExchange.Smart;
            contract.Currency = TwsCurrency.Usd;
            contract.PrimaryExch = TwsExchange.Island;

            Order order = new Order
            {
                Action = "BUY",
                OrderType = "LMT",
                TotalQuantity = 1,
                LmtPrice = 1,
            };

            int orderId = await nextOrderIdController.GetNextValidIdAsync();
            bool successfullyPlaced = await orderPlacementController.PlaceOrderAsync(orderId, contract, order);
            Thread.Sleep(1000); // TWS takes some time to put the order in the portfolio. Wait for it.

            var openOrders = await twsOpenOrdersController.RequestOpenOrders();
            openOrders.Count.Should().BeGreaterOrEqualTo(1);

            var daxOrders = openOrders.Where(orderEvent => orderEvent.Contract.Symbol == "MSFT").ToList();
            daxOrders.Count.Should().Be(1);
        }

        [Fact]
        public async Task TwsExecutionController_Should_ReturnOpenOrdersTwice()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            TwsOpenOrdersController twsOpenOrdersController = new TwsOpenOrdersController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsRequestIdGenerator twsRequestIdGenerator = new TwsRequestIdGenerator();
            TwsExecutionController executionController = new TwsExecutionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, twsRequestIdGenerator);

            await connectionController.EnsureConnectedAsync();

            // Create a position
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Stock;
            contract.Symbol = "MSFT";
            contract.Exchange = TwsExchange.Smart;
            contract.Currency = TwsCurrency.Usd;
            contract.PrimaryExch = TwsExchange.Island;

            Order order = new Order
            {
                Action = "BUY",
                OrderType = "LMT",
                TotalQuantity = 1,
                LmtPrice = 1,
            };

            int orderId = await nextOrderIdController.GetNextValidIdAsync();
            bool successfullyPlaced = await orderPlacementController.PlaceOrderAsync(orderId, contract, order);
            Thread.Sleep(1000); // TWS takes some time to put the order in the portfolio. Wait for it.

            var openOrders = await twsOpenOrdersController.RequestOpenOrders();
            openOrders.Count.Should().BeGreaterOrEqualTo(1);

            Thread.Sleep(5005);

            var openOrders2 = await twsOpenOrdersController.RequestOpenOrders();
            openOrders2.Count.Should().BeGreaterOrEqualTo(1);
        }
    }
}
