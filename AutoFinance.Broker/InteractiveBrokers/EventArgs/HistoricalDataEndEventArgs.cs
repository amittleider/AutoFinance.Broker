// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The historical data end event args
    /// </summary>
    public class HistoricalDataEndEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoricalDataEndEventArgs"/> class.
        /// </summary>
        /// <param name="requestId">The request Id</param>
        /// <param name="start">The start</param>
        /// <param name="end">The end</param>
        public HistoricalDataEndEventArgs(int requestId, string start, string end)
        {
            this.RequestId = requestId;
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Gets the request Id
        /// </summary>
        public int RequestId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the start
        /// </summary>
        public string Start
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the end
        /// </summary>
        public string End
        {
            get;
            private set;
        }
    }
}
