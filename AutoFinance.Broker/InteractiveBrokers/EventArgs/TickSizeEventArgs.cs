// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// Events container for data from the tickSize TWS endpoint
    /// </summary>
    public class TickSizeEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickSizeEventArgs"/> class.
        /// </summary>
        /// <param name="tickerId">The ticker id</param>
        /// <param name="field">The ticker field</param>
        /// <param name="size">The size</param>
        public TickSizeEventArgs(int tickerId, int field, decimal size)
        {
            this.TickerId = tickerId;
            this.Field = field;
            this.Size = size;
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
        /// Gets the size
        /// </summary>
        public decimal Size
        {
            get;
            private set;
        }
    }
}
