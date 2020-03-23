// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.EventArgs
{
    /// <summary>
    /// Events container for data from the tickNews TWS endpoint
    /// </summary>
    public class TickNewsEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickNewsEventArgs"/> class.
        /// </summary>
        /// <param name="tickerId">The ticker id</param>
        /// <param name="timeStamp">The timestamp</param>
        /// <param name="providerCode">The provider code</param>
        /// <param name="articleId">The article ID</param>
        /// <param name="headline">The headline</param>
        /// <param name="extraData">Any extra data</param>
        public TickNewsEventArgs(int tickerId, long timeStamp, string providerCode, string articleId, string headline, string extraData)
        {
            this.TickerId = tickerId;
            this.TimeStamp = timeStamp;
            this.ProviderCode = providerCode;
            this.ArticleId = articleId;
            this.Headline = headline;
            this.ExtraData = extraData;
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
        /// Gets the timestamp
        /// </summary>
        public long TimeStamp
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the provider code
        /// </summary>
        public string ProviderCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the article id
        /// </summary>
        public string ArticleId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the headline
        /// </summary>
        public string Headline
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets any extra data
        /// </summary>
        public string ExtraData
        {
            get;
            private set;
        }
    }
}
