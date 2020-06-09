// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// Events container for data from the tickString TWS endpoint
    /// </summary>
    public class TickStringEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickStringEventArgs"/> class.
        /// </summary>
        /// <param name="tickerId">The ticker id</param>
        /// <param name="field">The ticker field</param>
        /// <param name="tickString">The string value</param>
        public TickStringEventArgs(int tickerId, int field, string tickString)
        {
            this.TickerId = tickerId;
            this.Field = field;
            this.TickString = tickString;
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
        /// Gets the string value
        /// </summary>
        public string TickString
        {
            get;
            private set;
        }
    }
}
