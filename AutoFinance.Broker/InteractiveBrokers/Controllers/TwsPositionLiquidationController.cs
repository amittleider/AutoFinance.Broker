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
        private TwsConnectionController connectionController;
        private TwsPositionsController positionsController;
        private ITwsNextOrderIdController nextOrderIdController;
        private TwsOrderPlacementController orderPlacementController;
        private TwsPortfolioOrderCancellationController portfolioOrderCancellationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsPositionLiquidationController"/> class.
        /// </summary>
        /// <param name="twsConnectionController">The connection controller</param>
        /// <param name="positionsController">The position controller</param>
        /// <param name="nextOrderIdController">The order Id controller</param>
        /// <param name="orderPlacementController">The order placement controller</param>
        /// <param name="portfolioOrderCancellationController">The portfolio order cancellation controller</param>
        public TwsPositionLiquidationController(
            TwsConnectionController twsConnectionController,
            TwsPositionsController positionsController,
            ITwsNextOrderIdController nextOrderIdController,
            TwsOrderPlacementController orderPlacementController,
            TwsPortfolioOrderCancellationController portfolioOrderCancellationController)
        {
            this.connectionController = twsConnectionController;
            this.positionsController = positionsController;
            this.nextOrderIdController = nextOrderIdController;
            this.orderPlacementController = orderPlacementController;
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
            await this.connectionController.EnsureConnectedAsync();

            // Close any outstanding orders
            await this.portfolioOrderCancellationController.CancelOrders(symbol);

            // Get the number of shares and direction
            var positions = await this.positionsController.RequestPositions();
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

            int liquidationOrderId = await this.nextOrderIdController.GetNextValidIdAsync();
            bool success = await this.orderPlacementController.PlaceOrderAsync(liquidationOrderId, position.Contract, order);

            return success;
        }
    }
}
