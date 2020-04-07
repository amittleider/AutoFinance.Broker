// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using AutoFinance.Broker.InteractiveBrokers.Exceptions;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Finds and cancels orders in the portfolio
    /// </summary>
    public class TwsPortfolioOrderCancellationController
    {
        private ITwsControllerBase twsControllerBase;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsPortfolioOrderCancellationController"/> class.
        /// </summary>
        /// <param name="twsControllerBase">The tws base controller</param>
        public TwsPortfolioOrderCancellationController(ITwsControllerBase twsControllerBase)
        {
            this.twsControllerBase = twsControllerBase;
        }

        /// <summary>
        /// Cancels all orders with the given symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> CancelOrders(string symbol)
        {
            await this.twsControllerBase.EnsureConnectedAsync();

            var openOrders = await this.twsControllerBase.RequestOpenOrders();
            var openOrdersForSymbol = openOrders.Where(orderEvent => orderEvent.Contract.Symbol == symbol).Select(o => o.OrderId).Distinct().ToList();

            if (openOrdersForSymbol.Count == 0)
            {
                return false;
            }

            bool success = true;
            foreach (var openOrder in openOrdersForSymbol)
            {
                try
                {
                    success &= await this.twsControllerBase.CancelOrderAsync(openOrder);
                }
                catch (TwsException)
                {
                }
            }

            return success;
        }
    }
}
