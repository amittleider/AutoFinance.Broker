// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    using IBApi;

    /// <summary>
    /// Events container for data from the tickPrice TWS endpoint
    /// </summary>
    public class TickPriceEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickPriceEventArgs"/> class.
        /// </summary>
        /// <param name="tickerId">The ticker id</param>
        /// <param name="field">The ticker field</param>
        /// <param name="price">The price</param>
        /// <param name="attribs">The price attributes</param>
        public TickPriceEventArgs(int tickerId, int field, double price, TickAttrib attribs)
        {
            this.TickerId = tickerId;
            this.Field = field;
            this.Price = price;
            this.Attribs = attribs;
        }

        /// <summary>
        /// Gets the ticker id
        /// </summary>
        public int TickerId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the field
        /// </summary>
        public int Field
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the price
        /// </summary>
        public double Price
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the price attributes
        /// </summary>
        public TickAttrib Attribs
        {
            get;
            private set;
        }
    }
}
