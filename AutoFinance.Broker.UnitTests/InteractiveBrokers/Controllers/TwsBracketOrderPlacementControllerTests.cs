namespace AutoFinance.Broker.UnitTests.InteractiveBrokers.Controllers
{
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using AutoFinance.Broker.InteractiveBrokers.Controllers;
    using FluentAssertions;
    using IBApi;
    using Moq;
    using Xunit;

    public class TwsBracketOrderPlacementControllerTests
    {
        [Fact]
        public async Task Should_PlaceSimpleBracketOrder()
        {
            // Setup

            // Initialize the contract to trade
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Stock;
            contract.Symbol = "MSFT";
            contract.Exchange = TwsExchange.Smart;
            contract.PrimaryExch = TwsExchange.Island;

            // Initialize the expected orders to be placed
            Order entryOrder = new Order()
            {
                Action = TwsOrderActions.Buy,
                OrderType = TwsOrderType.PegToMarket,
                TotalQuantity = 100,
                Transmit = false,
                Tif = "GTC",
                PermId = 234234, // Set a random perm id unique per order, it's only for the unit test - normally TWS sets this value
            };

            Order takeProfit = new Order()
            {
                Action = TwsOrderActions.Sell,
                OrderType = TwsOrderType.Limit,
                TotalQuantity = 100,
                LmtPrice = 190,
                ParentId = 1,
                Transmit = false,
                Tif = "GTC",
                PermId = 52345,
            };

            Order stopLoss = new Order()
            {
                Action = TwsOrderActions.Sell,
                OrderType = TwsOrderType.StopLoss,
                TotalQuantity = 100,
                AuxPrice = 150,
                ParentId = 1,
                Transmit = true,
                Tif = "GTC",
                PermId = 615,
            };

            // Initialize the controllers
            Mock<ITwsControllerBase> mockControllerBase = new Mock<ITwsControllerBase>();
            mockControllerBase.SetupSequence(mock => mock.GetNextValidIdAsync())
                .ReturnsAsync(1)
                .ReturnsAsync(2)
                .ReturnsAsync(3);

            mockControllerBase.Setup(mock => mock.PlaceOrderAsync(1, contract, entryOrder)).ReturnsAsync(true);
            mockControllerBase.Setup(mock => mock.PlaceOrderAsync(2, contract, takeProfit)).ReturnsAsync(true);
            mockControllerBase.Setup(mock => mock.PlaceOrderAsync(3, contract, stopLoss)).ReturnsAsync(true);
            mockControllerBase.Setup(mock => mock.EnsureConnectedAsync()).Returns(Task.CompletedTask);

            // Initialize the order details
            string entryAction = TwsOrderActions.Buy;
            double quantity = 100;
            double takePrice = 190;
            double stopPrice = 150;

            var twsBracketOrderPlacementController = new TwsBracketOrderPlacementController(mockControllerBase.Object);

            // Call
            bool orderAck = await twsBracketOrderPlacementController.PlaceBracketOrder(contract, entryAction, quantity, takePrice, stopPrice);

            // Asssert
            orderAck.Should().BeTrue();

            mockControllerBase.VerifyAll();
        }
    }
}
