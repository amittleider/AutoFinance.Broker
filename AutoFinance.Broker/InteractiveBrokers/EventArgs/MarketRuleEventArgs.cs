// Licensed under the Apache License, Version 2.0.

using IBApi;

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The event arguments for the account download end event
    /// </summary>
    public class MarketRuleEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketRuleEventArgs"/> class.
        /// </summary>
        /// <param name="marketRuleId">The market rule id.</param>
        /// <param name="priceIncrements">Minimum price increments.</param>
        public MarketRuleEventArgs(int marketRuleId, PriceIncrement[] priceIncrements)
       {
           this.MarketRuleId = marketRuleId;
           this.PriceIncrements = priceIncrements;
       }

        /// <summary>
        /// Gets the request id.
        /// </summary>
        public int MarketRuleId { get; private set; }

        /// <summary>
        /// Gets the price increments
        /// </summary>
        public PriceIncrement[] PriceIncrements { get; private set; }
    }
}
