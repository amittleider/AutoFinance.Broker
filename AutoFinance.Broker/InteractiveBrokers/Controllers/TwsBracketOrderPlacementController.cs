// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System.Threading.Tasks;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using IBApi;

    /// <summary>
    /// A controller to place bracket orders on TWS.
    /// </summary>
    public class TwsBracketOrderPlacementController
    {
        private ITwsConnectionController connectionController;
        private ITwsNextOrderIdController nextOrderIdController;
        private ITwsOrderPlacementController orderPlacementController;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsBracketOrderPlacementController"/> class.
        /// </summary>
        /// <param name="connectionController">The connection controller</param>
        /// <param name="nextOrderIdController">The order ID generation controller</param>
        /// <param name="orderPlacementController">The base order placement controller</param>
        public TwsBracketOrderPlacementController(ITwsConnectionController connectionController, ITwsNextOrderIdController nextOrderIdController, ITwsOrderPlacementController orderPlacementController)
        {
            this.connectionController = connectionController;
            this.nextOrderIdController = nextOrderIdController;
            this.orderPlacementController = orderPlacementController;
        }

        /// <summary>
        /// Place a bracket order on TWS.
        /// The entry is a REL order (Market Peg), exits are stop loss and limit orders at the specified prices.
        /// </summary>
        /// <param name="contract">The contract to trade</param>
        /// <param name="entryAction">The entry action -- buy, sell, sshort</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="takePrice">The take price</param>
        /// <param name="stopPrice">The stop price</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<bool> PlaceBracketOrder(Contract contract, string entryAction, double quantity, double takePrice, double stopPrice)
        {
            await this.connectionController.EnsureConnectedAsync();

            // Generate the order IDs
            int entryOrderId = await this.nextOrderIdController.GetNextValidIdAsync();
            var takeProfitOrderId = await this.nextOrderIdController.GetNextValidIdAsync();
            var stopOrderId = await this.nextOrderIdController.GetNextValidIdAsync();

            // Initialize the order
            Order entryOrder = new Order()
            {
                Action = entryAction,
                OrderType = TwsOrderType.PegToMarket,
                TotalQuantity = quantity,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = false,
            };

            Order takeProfit = new Order()
            {
                Action = TwsOrderActions.Reverse(entryAction),
                OrderType = TwsOrderType.Limit,
                TotalQuantity = quantity,
                LmtPrice = takePrice,
                ParentId = entryOrderId,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = false,
            };

            Order stopLoss = new Order()
            {
                Action = TwsOrderActions.Reverse(entryAction),
                OrderType = TwsOrderType.StopLoss,
                TotalQuantity = quantity,
                AuxPrice = stopPrice,
                ParentId = entryOrderId,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = true,
            };

            var entryOrderAckTask = this.orderPlacementController.PlaceOrderAsync(entryOrderId, contract, entryOrder);
            var takeProfitOrderAckTask = this.orderPlacementController.PlaceOrderAsync(takeProfitOrderId, contract, takeProfit);
            var stopOrderAckTask = this.orderPlacementController.PlaceOrderAsync(stopOrderId, contract, stopLoss);
            Task.WaitAll(entryOrderAckTask, takeProfitOrderAckTask, stopOrderAckTask);

            return entryOrderAckTask.Result && takeProfitOrderAckTask.Result && stopOrderAckTask.Result;
        }

        /// <summary>
        /// Places a bracket order with a limit order entry
        /// </summary>
        /// <param name="contract">The contract</param>
        /// <param name="entryAction">Buy/sell/ssell</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="entryOrderPrice">The limit order entry price</param>
        /// <param name="takePrice">The take profit price</param>
        /// <param name="stopPrice">The stop loss price</param>
        /// <returns>True if the orders are correctly placed</returns>
        public async Task<bool> PlaceBracketOrder(Contract contract, string entryAction, double quantity, double entryOrderPrice, double takePrice, double stopPrice)
        {
            await this.connectionController.EnsureConnectedAsync();

            // Generate the order IDs
            int entryOrderId = await this.nextOrderIdController.GetNextValidIdAsync();
            var takeProfitOrderId = await this.nextOrderIdController.GetNextValidIdAsync();
            var stopOrderId = await this.nextOrderIdController.GetNextValidIdAsync();

            // Initialize the order
            Order entryOrder = new Order()
            {
                Action = entryAction,
                OrderType = TwsOrderType.Limit,
                TotalQuantity = quantity,
                LmtPrice = entryOrderPrice,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = false,
            };

            Order takeProfit = new Order()
            {
                Action = TwsOrderActions.Reverse(entryAction),
                OrderType = TwsOrderType.Limit,
                TotalQuantity = quantity,
                LmtPrice = takePrice,
                ParentId = entryOrderId,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = false,
            };

            Order stopLoss = new Order()
            {
                Action = TwsOrderActions.Reverse(entryAction),
                OrderType = TwsOrderType.StopLoss,
                TotalQuantity = quantity,
                AuxPrice = stopPrice,
                ParentId = entryOrderId,
                Tif = TwsTimeInForce.GoodTillClose,
                Transmit = true,
            };

            var entryOrderAckTask = this.orderPlacementController.PlaceOrderAsync(entryOrderId, contract, entryOrder);
            var takeProfitOrderAckTask = this.orderPlacementController.PlaceOrderAsync(takeProfitOrderId, contract, takeProfit);
            var stopOrderAckTask = this.orderPlacementController.PlaceOrderAsync(stopOrderId, contract, stopLoss);
            Task.WaitAll(entryOrderAckTask, takeProfitOrderAckTask, stopOrderAckTask);

            return entryOrderAckTask.Result && takeProfitOrderAckTask.Result && stopOrderAckTask.Result;
        }
    }
}
