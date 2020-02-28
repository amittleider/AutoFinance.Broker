using AutoFinance.Broker.InteractiveBrokers;
using AutoFinance.Broker.InteractiveBrokers.Constants;
using AutoFinance.Broker.InteractiveBrokers.Controllers;
using FluentAssertions;
using IBApi;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
{
    public class TwsPortfolioOrderCancellationControllerTests
    {
        [Fact]
        public async Task Should_CancelOrders()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            TwsOpenOrdersController twsOpenOrdersController = new TwsOpenOrdersController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderCancelationController orderCancellationController = new TwsOrderCancelationController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsPortfolioOrderCancellationController twsPortfolioOrderCancellationController = new TwsPortfolioOrderCancellationController(connectionController, twsOpenOrdersController, orderCancellationController);

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

            // Placea  couple of orders
            int orderId = await nextOrderIdController.GetNextValidIdAsync();
            await orderPlacementController.PlaceOrderAsync(orderId, contract, order);

            orderId = await nextOrderIdController.GetNextValidIdAsync();
            await orderPlacementController.PlaceOrderAsync(orderId, contract, order);
            Thread.Sleep(1000); // TWS takes some time to put the order in the portfolio. Wait for it.

            // Cancel them all
            var success = await twsPortfolioOrderCancellationController.CancelOrders("MSFT");

            success.Should().BeTrue();
        }
    }
}
