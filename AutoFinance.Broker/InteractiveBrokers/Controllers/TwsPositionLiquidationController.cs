// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using IBApi;

    /// <summary>
    /// Liquidates a position
    /// </summary>
    public class TwsPositionLiquidationController
    {
        private ITwsControllerBase twsControllerBase;
        private TwsPortfolioOrderCancellationController portfolioOrderCancellationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsPositionLiquidationController"/> class.
        /// </summary>
        /// <param name="twsControllerBase">The base tws controller</param>
        /// <param name="portfolioOrderCancellationController">The portfolio order cancellation controller</param>
        public TwsPositionLiquidationController(
            ITwsControllerBase twsControllerBase,
            TwsPortfolioOrderCancellationController portfolioOrderCancellationController)
        {
            this.twsControllerBase = twsControllerBase;
            this.portfolioOrderCancellationController = portfolioOrderCancellationController;
        }

        /// <summary>
        /// Liquidates the position with the given symbol to the given exchange
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="exchange">The exchange to send the order to. Can remove this param?</param>
        /// <returns>True if successful</returns>
        public async Task<bool> LiquidatePosition(string symbol, string exchange)
        {
            await this.twsControllerBase.EnsureConnectedAsync();

            // Close any outstanding orders
            await this.portfolioOrderCancellationController.CancelOrders(symbol);

            // Get the number of shares and direction
            var positions = await this.twsControllerBase.RequestPositions();
            var position = positions.Where(p => p.Contract.Symbol == symbol && p.Position != 0).FirstOrDefault();

            if (position == null)
            {
                return false;
            }

            // Use a REL order to exit
            string liquidationOrderDirection = string.Empty;
            if (position.Position > 0)
            {
                liquidationOrderDirection = TwsOrderActions.Sell;
            }

            if (position.Position < 0)
            {
                liquidationOrderDirection = TwsOrderActions.Buy;
            }

            Order order = new Order()
            {
                Action = liquidationOrderDirection,
                OrderType = TwsOrderType.PegToMarket,
                TotalQuantity = Math.Abs(position.Position),
            };

            position.Contract.Exchange = exchange; // TWS does not save the original traded exchange on the contract.

            int liquidationOrderId = await this.twsControllerBase.GetNextValidIdAsync();
            bool success = await this.twsControllerBase.PlaceOrderAsync(liquidationOrderId, position.Contract, order);

            return success;
        }
    }
}
