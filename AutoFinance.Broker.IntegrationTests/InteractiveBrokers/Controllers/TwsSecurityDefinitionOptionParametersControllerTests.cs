using AutoFinance.Broker.InteractiveBrokers;
using AutoFinance.Broker.InteractiveBrokers.Constants;
using AutoFinance.Broker.InteractiveBrokers.Controllers;
using FluentAssertions;
using IBApi;
using System.Linq;
using System.Threading.Tasks;
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

            ////// If you want, you can request the contract details from this info or get historical data for it
            ////Contract option = new Contract()
            ////{
            ////    SecType = TwsContractSecType.Option,
            ////    Symbol = "MSFT",
            ////    Exchange = "SMART",
            ////    Strike = 150,
            ////    LastTradeDateOrContractMonth = securityDefinitions[0].Expirations.First(), // March 27, 20
            ////    Right = "C",
            ////    Multiplier = securityDefinitions[0].Multiplier,
            ////    Currency = TwsCurrency.Usd,
            ////};

            ////var optionContractDetails = await twsContractDetailsController.GetContractAsync(option);
            ////var queryTime = DateTime.Now;
            ////List<HistoricalDataEventArgs> historicalDataEvents = await twsHistoricalDataController.GetHistoricalDataAsync(option, queryTime, TwsDuration.OneMonth, TwsBarSizeSetting.OneMinute, TwsHistoricalDataRequestType.Trades);
        }
    }
}
