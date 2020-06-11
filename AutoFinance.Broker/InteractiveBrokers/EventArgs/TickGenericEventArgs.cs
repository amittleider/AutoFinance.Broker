// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// Events container for data from the tickGeneric TWS endpoint
    /// </summary>
    public class TickGenericEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickGenericEventArgs"/> class.
        /// </summary>
        /// <param name="tickerId">The ticker id</param>
        /// <param name="field">The ticker field</param>
        /// <param name="value">The value</param>
        public TickGenericEventArgs(int tickerId, int field, double value)
        {
            this.TickerId = tickerId;
            this.Field = field;
            this.Value = value;
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
        /// Gets the value
        /// </summary>
        public double Value
        {
            get;
            private set;
        }
    }
}
