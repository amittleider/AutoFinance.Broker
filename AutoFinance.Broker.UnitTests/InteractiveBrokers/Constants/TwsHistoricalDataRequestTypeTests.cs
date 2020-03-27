using AutoFinance.Broker.InteractiveBrokers.Constants;
using FluentAssertions;
using Xunit;

namespace AutoFinance.Broker.UnitTests.InteractiveBrokers.Constants
{
    public class TwsHistoricalDataRequestTypeTests
    {
        [Fact]
        public void Test()
        {
            TwsHistoricalDataRequestType.Bid.ToTwsParameter().Should().Be("BID");
        }
    }
}
