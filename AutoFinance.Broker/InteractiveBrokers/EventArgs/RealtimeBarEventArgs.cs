// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    using Newtonsoft.Json;

    /// <summary>
    /// This class holds the event arguments from TWS Realtime Bar events
    /// </summary>
    public class RealtimeBarEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RealtimeBarEventArgs"/> class.
        /// </summary>
        /// <param name="requestId">The request Id</param>
        /// <param name="time">The time</param>
        /// <param name="open">The open price</param>
        /// <param name="high">The high price</param>
        /// <param name="low">The low price</param>
        /// <param name="close">The close price</param>
        /// <param name="volume">The volume</param>
        /// <param name="wap">The weighted average price</param>
        /// <param name="count">The number of trades during the period</param>
        public RealtimeBarEventArgs(int requestId, long time, double open, double high, double low, double close, decimal volume, decimal wap, int count)
        {
            this.RequestId = requestId;
            this.Time = time;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;
            this.Wap = wap;
            this.Count = count;
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
        /// Gets the time
        /// </summary>
        public long Time
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the open
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
        /// Gets the close
        /// </summary>
        public double Close
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the volume
        /// </summary>
        public decimal Volume
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the weighted average price
        /// </summary>
        public decimal Wap
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
        /// The equals method
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            return obj is RealtimeBarEventArgs args &&
                   this.RequestId == args.RequestId &&
                   this.Time == args.Time &&
                   this.Open == args.Open &&
                   this.High == args.High &&
                   this.Low == args.Low &&
                   this.Close == args.Close &&
                   this.Volume == args.Volume &&
                   this.Wap == args.Wap &&
                   this.Count == args.Count;
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            var hashCode = -49973705;
            hashCode = (hashCode * -1521134295) + this.RequestId.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Time.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Open.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.High.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Low.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Close.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Volume.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Wap.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Count.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Convert this object to a Json string.
        /// </summary>
        /// <returns>This object as a serialized string</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
