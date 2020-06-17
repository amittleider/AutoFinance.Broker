using AutoFinance.Broker.InteractiveBrokers;
using AutoFinance.Broker.InteractiveBrokers.Constants;
using AutoFinance.Broker.InteractiveBrokers.Controllers;
using AutoFinance.Broker.InteractiveBrokers.EventArgs;
using AutoFinance.Broker.InteractiveBrokers.Wrappers;
using FluentAssertions;
using IBApi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
{
    public class TwsControllerTests
    {
        [Fact]
        public async Task Should_PlaceBracketOrderWithStopLimit()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
            ITwsController twsController = twsObjectFactory.TwsController;

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
    }
}
