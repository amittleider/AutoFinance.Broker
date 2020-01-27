// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// The event arguments for TWS historical data events
    /// </summary>
    public class HistoricalDataEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoricalDataEventArgs"/> class.
        /// </summary>
        /// <param name="requestId">The request Id</param>
        /// <param name="date">The date</param>
        /// <param name="open">The open price</param>
        /// <param name="high">The high price</param>
        /// <param name="low">The low price</param>
        /// <param name="close">The close price</param>
        /// <param name="volume">The volume</param>
        /// <param name="count">The count</param>
        /// <param name="wap">The weighted average price</param>
        /// <param name="hasGaps">If the data has gaps</param>
        public HistoricalDataEventArgs(int requestId, string date, double open, double high, double low, double close, int volume, int count, double wap, bool hasGaps)
        {
            this.RequestId = requestId;
            this.Date = date;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;
            this.Count = count;
            this.Wap = wap;
            this.HasGaps = hasGaps;
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
        /// Gets the date
        /// </summary>
        public string Date
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the open price
        /// </summary>
        public double Open
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the high price
        /// </summary>
        public double High
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the low price
        /// </summary>
        public double Low
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the close price
        /// </summary>
        public double Close
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the volume
        /// </summary>
        public int Volume
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the count
        /// </summary>
        public int Count
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the weighted average price
        /// </summary>
        public double Wap
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the data has gaps
        /// </summary>
        public bool HasGaps
        {
            get;
            private set;
        }
    }
}
