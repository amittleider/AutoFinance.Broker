namespace AutoFinance.Broker.UnitTests.InteractiveBrokers.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using AutoFinance.Broker.InteractiveBrokers.Controllers;
    using AutoFinance.Broker.InteractiveBrokers.EventArgs;
    using AutoFinance.Broker.InteractiveBrokers.Wrappers;
    using FluentAssertions;
    using IBApi;
    using Moq;
    using Xunit;

    public class TwsControllerTests
    {
        [Fact]
        public void Should_PlaceSimpleBracketOrder()
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

            var twsController = new TwsController(mockControllerBase.Object);

            // Call
            bool orderAck = twsController.PlaceBracketOrder(contract, entryAction, quantity, takePrice, stopPrice).ConfigureAwait(false).GetAwaiter().GetResult();

            // Asssert
            orderAck.Should().BeTrue();

            mockControllerBase.VerifyAll();
        }

        /// <summary>
        /// Test that contract details are correctly returned from the ContractDetailsController.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public void ContractDetailsController_Should_ReturnValidContractAsync_UnderNormalCircumstances()
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

            TwsControllerBase contractDetailsController = new TwsControllerBase(mockTwsClientSocket.Object, mockTwsCallbackHandler.Object, "mock", 0, 0);

            // Call
            List<ContractDetails> actualContractDetails = contractDetailsController.GetContractAsync(contract).ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert
            actualContractDetails[0].Should().Be(expectedContractDetails);
        }

        /// <summary>
        /// Test that contract details are correctly returned from the ContractDetailsController.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public void OrderPlacementController_Should_PlaceOrdersSuccessfully()
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

            TwsControllerBase contractDetailsController = new TwsControllerBase(mockTwsClientSocket.Object, mockTwsCallbackHandler.Object, "mock", 0, 0);

            // Call
            List<ContractDetails> actualContractDetails = contractDetailsController.GetContractAsync(contract).ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert
            actualContractDetails[0].Should().Be(expectedContractDetails);
        }

        [Fact]
        public async Task PlaceBracketForExisting_WithLongPos_Should_PlaceBuyBracket()
        {
            // Setup

            // Initialize the contract to trade
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Stock;
            contract.Symbol = "MSFT";
            contract.Exchange = TwsExchange.Smart;
            contract.PrimaryExch = TwsExchange.Island;

            // Initialize the controllers
            Mock<ITwsControllerBase> mockControllerBase = new Mock<ITwsControllerBase>();
            mockControllerBase.SetupSequence(mock => mock.GetNextValidIdAsync())
                .ReturnsAsync(1)
                .ReturnsAsync(2);

            var expectedTakeProfit = It.Is<Order>(o => o.TotalQuantity == 900 &&
                o.Action == TwsOrderActions.Sell &&
                o.OrderType == TwsOrderType.StopLimit &&
                o.LmtPrice == 100 &&
                o.OcaType == (int)TwsOcaType.CancelAllRemainingOrdersWithBlock);
            var expectedStopLoss = It.Is<Order>(
                o => o.TotalQuantity == 900 && 
                o.Action == TwsOrderActions.Sell && 
                o.OrderType == TwsOrderType.StopLimit &&
                o.AuxPrice == 49 &&
                o.LmtPrice == 50 &&
                o.OcaGroup == expectedTakeProfit.OcaGroup &&
                o.OcaType == (int)TwsOcaType.CancelAllRemainingOrdersWithBlock);

            mockControllerBase.Setup(mock => mock.PlaceOrderAsync(1, contract, It.IsAny<Order>())).ReturnsAsync(true);
            mockControllerBase.Setup(mock => mock.PlaceOrderAsync(2, contract, It.IsAny<Order>())).ReturnsAsync(true);
            mockControllerBase.Setup(mock => mock.EnsureConnectedAsync()).Returns(Task.CompletedTask);

            List<OpenOrderEventArgs> openOrders = new List<OpenOrderEventArgs>();
            mockControllerBase.Setup(mock => mock.RequestOpenOrders()).ReturnsAsync(openOrders);

            List<PositionStatusEventArgs> positions = new List<PositionStatusEventArgs>();
            positions.Add(new PositionStatusEventArgs("acct", contract, 900, 75));

            mockControllerBase.Setup(mock => mock.RequestPositions()).ReturnsAsync(positions);

            // Initialize the order details
            var twsController = new TwsController(mockControllerBase.Object);

            // Call
            bool orderAck = await twsController.PlaceBracketForExistingPosition("MSFT", "SMART", 100, 50, 49);

            // Asssert
            orderAck.Should().BeTrue();

            mockControllerBase.VerifyAll();
        }
    }
}
