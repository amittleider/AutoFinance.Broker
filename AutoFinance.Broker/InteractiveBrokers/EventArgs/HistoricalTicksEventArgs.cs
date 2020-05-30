// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    using IBApi;

    /// <summary>
    /// The event arguments when a historical ticks is received
    /// </summary>
    public class HistoricalTicksEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoricalTicksEventArgs"/> class.
        /// </summary>
        /// <param name="reqId">The request identifier.</param>
        /// <param name="ticks">The historical ticks.</param>
        /// <param name="done">True if all data has been received. Otherwise false.</param>
        public HistoricalTicksEventArgs(int reqId, HistoricalTick[] ticks, bool done)
        {
            this.RequestId = reqId;
            this.Ticks = ticks;
            this.Done = done;
        }

        /// <summary>
        /// Gets the request id.
        /// </summary>
        public int RequestId { get; private set; }

        /// <summary>
        /// Gets the historical ticks.
        /// </summary>
        public HistoricalTick[] Ticks { get; private set; }

        /// <summary>
        /// Gets a value indicating whether true if all data has been received. Otherwise false.
        /// </summary>
        public bool Done { get; private set; }
    }
}
