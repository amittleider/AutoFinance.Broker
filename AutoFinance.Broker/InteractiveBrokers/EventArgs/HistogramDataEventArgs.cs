// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    using IBApi;

    /// <summary>
    /// The event arguments when a histogram is received.
    /// </summary>
    public class HistogramDataEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramDataEventArgs"/> class.
        /// </summary>
        /// <param name="reqId">The request identifier.</param>
        /// <param name="data">The histogram entries.</param>
        public HistogramDataEventArgs(int reqId, HistogramEntry[] data)
        {
            this.RequestId = reqId;
            this.Data = data;
        }

        /// <summary>
        /// Gets the request id.
        /// </summary>
        public int RequestId { get; private set; }
        
        /// <summary>
        /// Gets the histogram entries.
        /// </summary>
        public HistogramEntry[] Data { get; private set; }
    }
}
