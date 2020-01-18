namespace AutoFinance.Broker.UnitTests.InteractiveBrokers.Controllers
{
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using FluentAssertions;
    using Xunit;

    public class TwsOrderActionsTests
    {
        [Fact]
        public void Should_ReverseSignals()
        {
            TwsOrderActions.Reverse(TwsOrderActions.Buy).Should().Be(TwsOrderActions.Sell);
            TwsOrderActions.Reverse(TwsOrderActions.Sell).Should().Be(TwsOrderActions.Buy);
            TwsOrderActions.Reverse(TwsOrderActions.ShortSell).Should().Be(TwsOrderActions.Buy);
        }
    }
}
