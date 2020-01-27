// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Constants
{
    using System;

    /// <summary>
    /// Order actions
    /// </summary>
    public partial class TwsOrderActions
    {
        /// <summary>
        /// Buy
        /// </summary>
        public const string Buy = "BUY";

        /// <summary>
        /// Sell
        /// </summary>
        public const string Sell = "SELL";

        /// <summary>
        /// Short
        /// </summary>
        public const string ShortSell = "SSELL";

        /// <summary>
        /// Gets the opposite side of the order
        /// </summary>
        /// <param name="orderAction">The order action to reverse</param>
        /// <returns>The reverse action</returns>
        public static string Reverse(string orderAction)
        {
            if (orderAction == TwsOrderActions.Buy)
            {
                return TwsOrderActions.Sell;
            }

            if (orderAction == TwsOrderActions.Sell)
            {
                return TwsOrderActions.Buy;
            }

            if (orderAction == TwsOrderActions.ShortSell)
            {
                return TwsOrderActions.Buy;
            }

            throw new NotImplementedException($"Order action type {orderAction} not implemented.");
        }
    }
}
