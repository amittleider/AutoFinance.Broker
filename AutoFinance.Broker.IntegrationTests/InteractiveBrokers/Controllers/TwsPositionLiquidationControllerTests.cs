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
    public class TwsPositionLiquidationControllerTests
    {
        [Fact]
        public async Task Should_LiquidateLongPosition()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            TwsOpenOrdersController twsOpenOrdersController = new TwsOpenOrdersController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderCancelationController orderCancellationController = new TwsOrderCancelationController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsPortfolioOrderCancellationController twsPortfolioOrderCancellationController = new TwsPortfolioOrderCancellationController(connectionController, twsOpenOrdersController, orderCancellationController);
            TwsPositionsController positionsController = new TwsPositionsController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsPositionLiquidationController positionLiquidationController = new TwsPositionLiquidationController(
                connectionController,
                positionsController,
                nextOrderIdController,
                orderPlacementController,
                twsPortfolioOrderCancellationController);

            await connectionController.EnsureConnectedAsync();

            // Create a position
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Future;
            contract.Symbol = TwsSymbol.Dax;
            contract.Exchange = TwsExchange.Dtb;
            contract.Currency = TwsCurrency.Eur;
            contract.Multiplier = "25";
            contract.LastTradeDateOrContractMonth = "202003";

            Order order = new Order
            {
                Action = TwsOrderActions.Buy,
                OrderType = "MKT",
                TotalQuantity = 1,
            };

            // Place a couple of orders
            int orderId = await nextOrderIdController.GetNextValidIdAsync();
            await orderPlacementController.PlaceOrderAsync(orderId, contract, order);
            Thread.Sleep(1000); // TWS takes some time to put the order in the portfolio. Wait for it.

            // Liquidate the position
            bool success = await positionLiquidationController.LiquidatePosition(TwsSymbol.Dax, TwsExchange.Dtb);
            success.Should().BeTrue();
        }

        [Fact]
        public async Task Should_LiquidateShortPosition()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            TwsOpenOrdersController twsOpenOrdersController = new TwsOpenOrdersController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsOrderCancelationController orderCancellationController = new TwsOrderCancelationController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsPortfolioOrderCancellationController twsPortfolioOrderCancellationController = new TwsPortfolioOrderCancellationController(connectionController, twsOpenOrdersController, orderCancellationController);
            TwsPositionsController positionsController = new TwsPositionsController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
            TwsPositionLiquidationController positionLiquidationController = new TwsPositionLiquidationController(
                connectionController,
                positionsController,
                nextOrderIdController,
                orderPlacementController,
                twsPortfolioOrderCancellationController);

            await connectionController.EnsureConnectedAsync();

            // Create a position
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Future;
            contract.Symbol = TwsSymbol.Dax;
            contract.Exchange = TwsExchange.Dtb;
            contract.Currency = TwsCurrency.Eur;
            contract.Multiplier = "25";
            contract.LastTradeDateOrContractMonth = "202003";

            Order order = new Order
            {
                Action = TwsOrderActions.Sell,
                OrderType = "MKT",
                TotalQuantity = 1,
            };

            // Place a couple of orders
            int orderId = await nextOrderIdController.GetNextValidIdAsync();
            await orderPlacementController.PlaceOrderAsync(orderId, contract, order);
            Thread.Sleep(1000); // TWS takes some time to put the order in the portfolio. Wait for it.

            // Liquidate the position
            bool success = await positionLiquidationController.LiquidatePosition(TwsSymbol.Dax, TwsExchange.Dtb);
            success.Should().BeTrue();
        }
    }
}
