namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Finds and cancels orders in the portfolio
    /// </summary>
    public class TwsPortfolioOrderCancellationController
    {
        private ITwsConnectionController connectionController;
        private ITwsOpenOrdersController openOrdersController;
        private ITwsOrderCancelationController orderCancellationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwsPortfolioOrderCancellationController"/> class.
        /// </summary>
        /// <param name="connectionController">The connection controller</param>
        /// <param name="openOrdersController">The open orders controller</param>
        /// <param name="orderCancellationController">The order cancellation controller</param>
        public TwsPortfolioOrderCancellationController(ITwsConnectionController connectionController, ITwsOpenOrdersController openOrdersController, ITwsOrderCancelationController orderCancellationController)
        {
            this.connectionController = connectionController;
            this.openOrdersController = openOrdersController;
            this.orderCancellationController = orderCancellationController;
        }

        /// <summary>
        /// Cancels all orders with the given symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> CancelOrders(string symbol)
        {
            await this.connectionController.EnsureConnectedAsync();

            var openOrders = await this.openOrdersController.RequestOpenOrders();
            var openOrdersForSymbol = openOrders.Where(orderEvent => orderEvent.Contract.Symbol == symbol).ToList();

            if (openOrdersForSymbol.Count == 0)
            {
                return false;
            }

            bool success = true;
            foreach (var openOrder in openOrdersForSymbol)
            {
                success &= await this.orderCancellationController.CancelOrderAsync(openOrder.OrderId);
            }

            return success;
        }
    }
}
