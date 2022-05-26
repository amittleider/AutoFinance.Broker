using AutoFinance.Broker.InteractiveBrokers;
using AutoFinance.Broker.InteractiveBrokers.Constants;
using AutoFinance.Broker.InteractiveBrokers.Controllers;
using FluentAssertions;
using IBApi;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
{
    public class TwsControllerTests
    {
        [Fact]
        public async Task Should_PlaceBracketOrderWithStopLimit()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            TwsController twsController = twsObjectFactory.TwsController;

            await twsController.EnsureConnectedAsync();

            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
                Currency = TwsCurrency.Usd,
            };

            bool placed = await twsController.PlaceBracketOrder(contract, TwsOrderActions.Buy, 1, 10, 1000, 1, 0.9);

            placed.Should().BeTrue();

            await twsController.DisconnectAsync();
        }

        [Fact]
        public async Task Should_PlaceBracketOrderWithStopLimit_ForGrwgContract()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            TwsController twsController = twsObjectFactory.TwsController;

            await twsController.EnsureConnectedAsync();

            var orderId = await twsController.GetNextValidIdAsync();
            var orderId2 = await twsController.GetNextValidIdAsync();

            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "GRWG",
                Exchange = TwsExchange.Smart,
                PrimaryExch = TwsExchange.Island,
                Currency = TwsCurrency.Usd,
            };

            //Order order = new Order
            //{
            //    Action = "BUY",
            //    OrderType = "MKT",
            //    TotalQuantity = 1
            //};

            string group = Guid.NewGuid().ToString();

            Order order = new Order()
            {
                Action = "SELL",
                OrderType = TwsOrderType.Limit,
                TotalQuantity = 200,
                LmtPrice = 10,
                Tif = TwsTimeInForce.GoodTillClose,
                OcaType = (int)TwsOcaType.CancelAllRemainingOrdersWithBlock,
                OcaGroup = group,
                Transmit = true,
            };

            Order order2 = new Order()
            {
                Action = "SELL",
                OrderType = TwsOrderType.StopLimit,
                TotalQuantity = 200,
                AuxPrice = 4,
                LmtPrice = 3.9,
                Tif = TwsTimeInForce.GoodTillClose,
                OcaType = (int)TwsOcaType.CancelAllRemainingOrdersWithBlock,
                OcaGroup = group,
                Transmit = true,
            };

            bool placed = await twsController.PlaceOrderAsync(orderId, contract, order);
            bool placed2 = await twsController.PlaceOrderAsync(orderId2, contract, order2);

            placed.Should().BeTrue();

            await twsController.DisconnectAsync();
        }

        [Fact]
        public async Task Should_PlaceBracketForExistingPosition()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            TwsController twsController = twsObjectFactory.TwsController;

            await twsController.EnsureConnectedAsync();

            bool success = await twsController.PlaceBracketForExistingPosition("GRWG", "SMART", 6, 4, 3.9);

            success.Should().BeTrue();

            await twsController.DisconnectAsync();
        }

        [Fact]
        public async Task Should_CancelAllOrders()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", TestConstants.Port, 1);
            TwsController twsController = twsObjectFactory.TwsController;

            await twsController.EnsureConnectedAsync();

            bool success = await twsController.CancelAllOrders();

            success.Should().BeTrue();

            await twsController.DisconnectAsync();
        }
    }
}
