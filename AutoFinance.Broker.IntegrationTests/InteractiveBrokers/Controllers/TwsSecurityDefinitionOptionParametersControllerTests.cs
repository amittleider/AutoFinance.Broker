using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFinance.Broker.InteractiveBrokers;
using AutoFinance.Broker.InteractiveBrokers.Constants;
using AutoFinance.Broker.InteractiveBrokers.Controllers;
using AutoFinance.Broker.InteractiveBrokers.EventArgs;
using FluentAssertions;
using IBApi;
using Xunit;

namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers.Controllers
{
    public class TwsSecurityDefinitionOptionParametersControllerTests
    {
        [Fact]
        public async Task Should_GetOptionsContracts()
        {
            TwsObjectFactory twsObjectFactory = new TwsObjectFactory();

            TwsRequestIdGenerator twsRequestIdGenerator = new TwsRequestIdGenerator();
            TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
            TwsSecurityDefinitionOptionParametersController securityDefinitionController = new TwsSecurityDefinitionOptionParametersController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, twsRequestIdGenerator);
            TwsContractDetailsController twsContractDetailsController = new TwsContractDetailsController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, twsRequestIdGenerator);
            await connectionController.EnsureConnectedAsync();

            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Stock,
                Symbol = "MSFT",
                Exchange = TwsExchange.Smart,
            };

            // Get the contract details of the STOCK so that you can find the underlying security ID, required for the security definitions call.
            var contractDetails = await twsContractDetailsController.GetContractAsync(contract);
            var securityDefinitions = await securityDefinitionController.RequestSecurityDefinitionOptionParameters("MSFT", "", "STK", contractDetails.First().Contract.ConId);

            securityDefinitions.Count.Should().BeGreaterThan(1);

            // If you want, you can request the contract details from this info or get historical data for it
            ////Contract option = new Contract()
            ////{
            ////    SecType = TwsContractSecType.Option,
            ////    Symbol = "MSFT", 
            ////    Exchange = stuff[0].Exchange,
            ////    Strike = strike,
            ////    LastTradeDateOrContractMonth = expiration,
            ////    Right = "C",
            ////    Multiplier = stuff[0].Multiplier,
            ////    Currency = TwsCurrency.Usd,
            ////};

            ////var optionContractDetails = await twsContractDetailsController.GetContractAsync(option);
            ////string queryTime = DateTime.Now.AddMonths(-6).ToString("yyyyMMdd HH:mm:ss");
            ////List<HistoricalDataEventArgs> historicalDataEvents = await historicalDataController.GetHistoricalDataAsync(option, queryTime, "1 M", "1 day", "MIDPOINT");
        }
    }
}
