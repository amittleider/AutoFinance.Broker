// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Controllers
{
    using System.Threading.Tasks;
    using IBApi;

    /// <summary>
    /// The interface for the high level controller
    /// </summary>
    public interface ITwsController : ITwsControllerBase
    {
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
        Task<bool> PlaceBracketOrder(
            Contract contract, string entryAction, double quantity, double takePrice, double stopPrice);

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
        Task<bool> PlaceBracketOrder(
            Contract contract,
            string entryAction,
            double quantity,
            double entryOrderPrice,
            double takePrice,
            double stopPrice);

        /// <summary>
        /// Places a bracket order with a limit order entry and stop limit exit
        /// </summary>
        /// <param name="contract">The contract</param>
        /// <param name="entryAction">Buy/sell/ssell</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="entryOrderPrice">The limit order entry price</param>
        /// <param name="takePrice">The take profit price</param>
        /// <param name="stopActivationPrice">The stop loss price</param>
        /// <param name="stopLimitPrice">The price to put the limit after the stop activation price is touched</param>
        /// <returns>True if the orders are correctly placed</returns>
        Task<bool> PlaceBracketOrder(
            Contract contract,
            string entryAction,
            double quantity,
            double entryOrderPrice,
            double takePrice,
            double stopActivationPrice,
            double stopLimitPrice);

        /// <summary>
        /// Places a bracket order for a position that already exists.
        /// Useful to re-set expired GTC brackets.
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="exchange">The exchange</param>
        /// <param name="takePrice">The take price</param>
        /// <param name="stopActivationPrice">The stop activation price</param>
        /// <param name="stopLimitPrice">The stop limit price</param>
        /// <returns>True if the order placement succeeded, false otherwise</returns>
        Task<bool> PlaceBracketForExistingPosition(string symbol, string exchange, double takePrice, double stopActivationPrice, double stopLimitPrice);

        /// <summary>
        /// Cancels all orders with the given symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> CancelOrders(string symbol);

        /// <summary>
        /// Cancels all orders with the given symbol
        /// </summary>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> CancelAllOrders();

        /// <summary>
        /// Liquidates the position with the given symbol to the given exchange
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="exchange">The exchange to send the order to. Can remove this param?</param>
        /// <returns>True if successful</returns>
        Task<bool> LiquidatePosition(string symbol, string exchange);
    }
}
